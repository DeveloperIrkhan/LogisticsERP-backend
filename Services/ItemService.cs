using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Item;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class ItemService : ServiceBaseFunctions, IItemService

    {
        private readonly IGenericRepo<Item> _genericRepo;
        private readonly IItemRepo _itemRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ItemService(
            IGenericRepo<Item> genericRepo,
            IItemRepo itemRepo,
            IMapper mapper,
            AppDbContext context)
        {
            _genericRepo = genericRepo;
            _itemRepo = itemRepo;
            _mapper = mapper;
            _context = context;
        }


        public async Task<ApiResponse<ItemResponseDto>> CreateAsync(ItemCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return Fail<ItemResponseDto>("please provide the fields data.");

                var existing = await _itemRepo.GetByNameAsync(dto.ItemName);
                if (existing != null)
                    return Fail<ItemResponseDto>("An item with this name already exists in the catalog.");

                var item = _mapper.Map<Item>(dto);
                item.CurrentStock = dto.OpeningStock;
                item.IsActive = true;

                await _genericRepo.AddAsync(item);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<ItemResponseDto>(item), "Item created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemResponseDto>> UpdateAsync(string id, ItemUpdateDto dto)
        {
            try
            {
                var item = await _genericRepo.GetByIdAsync(id);
                if (item == null)
                    return Fail<ItemResponseDto>("Item not found.");

                if (dto.ItemName != null) item.ItemName = dto.ItemName;
                if (dto.ItemCategory.HasValue) item.ItemCategory = dto.ItemCategory.Value;
                if (dto.Unit.HasValue) item.Unit = dto.Unit.Value;
                if (dto.ReorderLevel.HasValue) item.ReorderLevel = dto.ReorderLevel.Value;
                if (dto.Description != null) item.Description = dto.Description;
                if (dto.IsActive.HasValue) item.IsActive = dto.IsActive.Value;

                await _genericRepo.Update(item);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<ItemResponseDto>(item), "Item updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<ItemResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var item = await _genericRepo.GetByIdAsync(id);
                if (item == null)
                    return Fail<ItemResponseDto>("Item not found.");

                return Ok(_mapper.Map<ItemResponseDto>(item), "Item fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<List<ItemResponseDto>>> GetAllAsync()
        {
            try
            {
                var items = await _genericRepo.GetAllAsync();
                var result = _mapper.Map<List<ItemResponseDto>>(items);
                return Ok(result, $"{result.Count} item(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var item = await _genericRepo.GetByIdAsync(id);
                if (item == null)
                    return Fail<bool>("Item not found.");

                var hasTransactions = await _context.ItemPurchases.AnyAsync(x => x.ItemId == id)
                    || await _context.ItemSales.AnyAsync(x => x.ItemId == id);

                if (hasTransactions)
                    return Fail<bool>("This item has purchase/sale history and cannot be deleted. You can mark it inactive instead.");

                await _genericRepo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "Item deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<List<ItemResponseDto>>> GetByCategoryAsync(ItemCategory itemCategory)
        {
            try
            {
                var items = await _itemRepo.GetByCategoryAsync(itemCategory);
                var result = _mapper.Map<List<ItemResponseDto>>(items);
                return Ok(result, $"{result.Count} item(s) found in category {itemCategory}.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<List<ItemResponseDto>>> GetActiveAsync()
        {
            try
            {
                var items = await _itemRepo.GetActiveAsync();
                var result = _mapper.Map<List<ItemResponseDto>>(items);
                return Ok(result, $"{result.Count} active item(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<List<ItemResponseDto>>> GetLowStockAsync()
        {
            try
            {
                var items = await _itemRepo.GetLowStockAsync();
                var result = _mapper.Map<List<ItemResponseDto>>(items);
                return Ok(result, $"{result.Count} item(s) at or below reorder level.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<List<ItemStockReportDto>>> GetStockReportAsync()
        {
            try
            {
                var items = await _context.Items
                    .Include(x => x.ItemPurchase)
                    .Include(x => x.Sales)
                    .ToListAsync();

                var report = items.Select(x => new ItemStockReportDto
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    ItemCategory = x.ItemCategory,
                    Unit = x.Unit,
                    CurrentStock = x.CurrentStock,
                    ReorderLevel = x.ReorderLevel,
                    IsLowStock = x.ReorderLevel.HasValue && x.CurrentStock <= x.ReorderLevel.Value,
                    TotalPurchasedQty = x.ItemPurchase
                        .Where(p => p.Status == ItemTransactionStatus.Approved 
                        || p.Status == ItemTransactionStatus.Paid)
                        .Sum(p => p.Quantity),
                    TotalSoldQty = x.Sales
                        .Where(s => s.Status == ItemTransactionStatus.Approved 
                        || s.Status == ItemTransactionStatus.Paid)
                        .Sum(s => s.Quantity),
                }).OrderBy(x => x.ItemName).ToList();

                return Ok(report, $"Stock report generated for {report.Count} item(s).");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemStockReportDto>>(ex.InnerException?.Message ?? ex.Message);
            }

        }
    }
}
