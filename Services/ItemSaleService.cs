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
    public class ItemSaleService : ServiceBaseFunctions, IItemSaleService
    {
        private readonly IGenericRepo<ItemSale> _genericRepo;
        private readonly IItemSaleRepo _itemsaleRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ItemSaleService(
            IGenericRepo<ItemSale> genericRepo,
            IItemSaleRepo itemSaleRepo,
            IMapper mapper,
            AppDbContext appDbContext
            )
        {
            _genericRepo = genericRepo;
            _itemsaleRepo = itemSaleRepo;
            _mapper = mapper;
            _context = appDbContext;
        }

        public async Task<ApiResponse<ItemSaleResponseDto>> CreateAsync(ItemSaleCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return Fail<ItemSaleResponseDto>("please provide field(s) data!");
                if (dto.Quantity < 0)
                    return Fail<ItemSaleResponseDto>("Quantity must be greater then 0");

                var item = await _context.Items.FindAsync(dto.ItemId);
                if (item == null)
                    return Fail<ItemSaleResponseDto>("Selected Item was not found in catalog.");
                if (item.CurrentStock < dto.Quantity)
                    return Fail<ItemSaleResponseDto>($"Insufficient stock. Available:" +
                        $" {item.CurrentStock} {item.Unit}, requested: {dto.Quantity}.");

                var sale = _mapper.Map<ItemSale>(dto);
                sale.TotalAmount = dto.Quantity * dto.UnitPrice;
                sale.Status = ItemTransactionStatus.Pending;
                await _genericRepo.AddAsync(sale);
                await _context.SaveChangesAsync();

                var result = await LoadResponseAsync(sale.ItemSaleId);
                return Ok(result!, "Sale recorded successfully and is pending approval.");



            }
            catch (Exception ex)
            {
                return Fail<ItemSaleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        public async Task<ApiResponse<ItemSaleResponseDto>> UpdateAsync(string id, ItemSaleUpdateDto dto)
        {
            try
            {
                var sale = await _genericRepo.GetByIdAsync(id);
                if (sale is null)
                    return Fail<ItemSaleResponseDto>("Sale Record not found.");

                if (dto.Quantity.HasValue || dto.UnitPrice.HasValue
                    && sale.Status != ItemTransactionStatus.Pending)
                    return Fail<ItemSaleResponseDto>("Quantity/Price can be Edit only if sale is pending.");

                if (dto.Quantity.HasValue) sale.Quantity = dto.Quantity.Value;
                if (dto.UnitPrice.HasValue) sale.UnitPrice = dto.UnitPrice.Value;
                if (dto.Quantity.HasValue || dto.UnitPrice.HasValue)
                    sale.TotalAmount = sale.Quantity * sale.UnitPrice;
                if (dto.SaleDate.HasValue) sale.SaleDate = dto.SaleDate.Value;
                if (dto.BuyerName != null) sale.BuyerName = dto.BuyerName;
                if (dto.InvoiceNumber != null) sale.InvoiceNumber = dto.InvoiceNumber;
                if (dto.PaymentMode.HasValue) sale.PaymentMode = dto.PaymentMode.Value;
                if (dto.VehicleId != null) sale.VehicleId = dto.VehicleId;
                if (dto.Notes != null) sale.Notes = dto.Notes;
                if (dto.ApprovedBy != null) sale.ApprovedBy = dto.ApprovedBy;
                if (dto.Status.HasValue) sale.Status = dto.Status.Value;

                await _genericRepo.Update(sale);
                await _context.SaveChangesAsync();
                var result = await LoadResponseAsync(id);
                return Ok(result!, "Sale updated successfully.");


            }
            catch (Exception ex)
            {
                return Fail<ItemSaleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        public async Task<ApiResponse<ItemSaleResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var result = await LoadResponseAsync(id);
                if (result == null)
                    return Fail<ItemSaleResponseDto>("Sale record not found.");

                return Ok(result, "Sale fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemSaleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }



        public async Task<ApiResponse<List<ItemSaleResponseDto>>> GetAllAsync()
        {
            try
            {
                var sale = await _context.ItemSales.Include(x => x.Item)
                            .OrderByDescending(x => x.SaleDate)
                            .ToListAsync();
                var result = MapList(sale);
                return Ok(result, $"{result.Count} sale record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemSaleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var sale = await _genericRepo.GetByIdAsync(id);
                if (sale == null)
                    return Fail<bool>("Sale record not found.");

                // If stock was already deducted (Approved/Paid), restore it before deleting.
                if (sale.Status == ItemTransactionStatus.Approved || sale.Status == ItemTransactionStatus.Paid)
                {
                    var item = await _context.Items.FindAsync(sale.ItemId);
                    //item?.CurrentStock += sale.Quantity;
                    if (item != null) item.CurrentStock += sale.Quantity;
                }

                await _genericRepo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "Sale deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemSaleResponseDto>>> GetByItemAsync(string itemId)
        {
            try
            {
                var sales = await _itemsaleRepo.GetByItemAsync(itemId);
                var result = MapList(sales);
                return Ok(result, $"{result.Count} sale record(s) found for item.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemSaleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemSaleResponseDto>> ApproveAsync(string id, string approvedBy)
        {
            try
            {
                var sale = await _genericRepo.GetByIdAsync(id);
                if (sale is null)
                    return Fail<ItemSaleResponseDto>("sale record not found.");

                if (sale.Status is ItemTransactionStatus.Approved || sale.Status is ItemTransactionStatus.Paid)
                    return Fail<ItemSaleResponseDto>("sale is already approved.");


                var item = await _context.Items.FindAsync(sale.ItemId);
                if (item is null)
                    return Fail<ItemSaleResponseDto>("Related item no longer existed in the catalog.");

                if (item.CurrentStock < sale.Quantity)
                    return Fail<ItemSaleResponseDto>($"Insufficient stock to approve this sale. Available: {item.CurrentStock} {item.Unit}, requested: {sale.Quantity}.");


                sale.Status = ItemTransactionStatus.Approved;
                sale.ApprovedBy = approvedBy;
                item.CurrentStock -= sale.Quantity;
                await _genericRepo.Update(sale);
                var result = await LoadResponseAsync(id);
                return Ok(result!, "Sale approved and stock updated successfully");
            }
            catch (Exception ex)
            {
                return Fail<ItemSaleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemSaleResponseDto>>> GetByVehicleAsync(string vehicleId)
        {
            try
            {
                var sale = await _itemsaleRepo.GetByVehicleAsync(vehicleId);
                var result = MapList(sale);
                return Ok(result, $"{result.Count} sale record(s) found for vehicle.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemSaleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemSaleResponseDto>>> GetByStatusAsync(ItemTransactionStatus status)
        {
            try
            {
                var sales = await _itemsaleRepo.GetByStatusAsync(status);
                var result = MapList(sales);
                return Ok(result, $"{result.Count} sale record(s) found with status {status}.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemSaleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemSaleResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                if (from > to)
                    return Fail<List<ItemSaleResponseDto>>("'From' date cannot be greater than 'To' date.");

                var sales = await _itemsaleRepo.GetByDateRangeAsync(from, to);
                var result = MapList(sales);
                return Ok(result, $"{result.Count} sale record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemSaleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemSaleResponseDto>> RejectAsync(string id, string approvedBy)
        {
            try
            {
                var sale = await _genericRepo.GetByIdAsync(id);
                if (sale == null)
                    return Fail<ItemSaleResponseDto>("Sale record not found.");

                if (sale.Status == ItemTransactionStatus.Rejected)
                    return Fail<ItemSaleResponseDto>("Sale is already rejected.");

                //If stock was already deducted(was Approved / Paid), restore it.
                if (sale.Status == ItemTransactionStatus.Approved || sale.Status == ItemTransactionStatus.Paid)
                {
                    var item = await _context.Items.FindAsync(sale.ItemId);
                    if (item != null) item.CurrentStock += sale.Quantity;
                }
                sale.Status = ItemTransactionStatus.Rejected;
                sale.ApprovedBy = approvedBy;

                await _genericRepo.Update(sale);
                await _context.SaveChangesAsync();

                var result = await LoadResponseAsync(id);
                return Ok(result!, "Sale rejected successfully.");


            }
            catch (Exception ex)
            {
                return Fail<ItemSaleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<ItemSaleMonthlyReportDto>> GetMonthlyReportAsync(int year, int month)
        {
            try
            {
                var sale = await _itemsaleRepo.GetByMonthAsync(year, month);
                var total = await _itemsaleRepo.GetTotalByMonthAsync(year, month);


                var report = new ItemSaleMonthlyReportDto
                {
                    Year = year,
                    Month = month,
                    TotalAmount = total,
                    TotalRecords = sale.Count
                };

                return Ok(report, $"Monthly sale report for {year}/{month} fetched successfully.");

            }
            catch (Exception ex)
            {
                return Fail<ItemSaleMonthlyReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemSaleResponseDto>> MarkPaidAsync(string id, string approvedBy)
        {
            try
            {
                var sale = await _genericRepo.GetByIdAsync(id);
                if (sale is null)
                    return Fail<ItemSaleResponseDto>("sale not found.");


                if (sale.Status != ItemTransactionStatus.Approved)
                    return Fail<ItemSaleResponseDto>("Only an approved sale can be marked as paid.");

                sale.Status = ItemTransactionStatus.Paid;
                sale.ApprovedBy = approvedBy;

                await _genericRepo.Update(sale);
                await _context.SaveChangesAsync();

                var result = await LoadResponseAsync(id);
                return Ok(result!, "Sale marked as paid successfully.");

            }
            catch (Exception ex)
            {
                return Fail<ItemSaleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        //helper function for map data
        private async Task<ItemSaleResponseDto?> LoadResponseAsync(string id)
        {
            var sale = await _context.ItemSales
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.ItemSaleId == id);

            return sale is null ? null : MapData(sale);
        }

        private static ItemSaleResponseDto MapData(ItemSale sale)
        {
            return new ItemSaleResponseDto
            {
                ItemSaleId = sale.ItemSaleId,
                ItemId = sale.ItemId,
                ItemName = sale.Item?.ItemName ?? string.Empty,
                Quantity = sale.Quantity,
                UnitPrice = sale.UnitPrice,
                TotalAmount = sale.TotalAmount,
                SaleDate = sale.SaleDate,
                BuyerName = sale.BuyerName,
                InvoiceNumber = sale.InvoiceNumber,
                PaymentMode = sale.PaymentMode,
                Status = sale.Status,
                VehicleId = sale.VehicleId,
                AddedBy = sale.AddedBy,
                ApprovedBy = sale.ApprovedBy,
                Notes = sale.Notes,
                CreatedAt = sale.CreatedAt,
            };
        }


        private static List<ItemSaleResponseDto> MapList(List<ItemSale> sales)
        {
            return sales.ConvertAll(MapData);
        }
    }
}
