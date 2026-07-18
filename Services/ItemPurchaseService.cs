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
    public class ItemPurchaseService : ServiceBaseFunctions, IItemPurchaseService

    {
        private readonly IGenericRepo<ItemPurchase> _genericRepo;
        private readonly IItemPurchaseRepo _purchaseRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ItemPurchaseService(
            IGenericRepo<ItemPurchase> genericRepo,
            IItemPurchaseRepo purchaseRepo,
            IMapper mapper,
            AppDbContext context)
        {
            _genericRepo = genericRepo;
            _purchaseRepo = purchaseRepo;
            _mapper = mapper;
            _context = context;
        }


        public async Task<ApiResponse<ItemPurchaseResponseDto>> CreateAsync(ItemPurchaseCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return Fail<ItemPurchaseResponseDto>("please provide the fields data.");
                if (dto.Quantity <= 0)
                    return Fail<ItemPurchaseResponseDto>("Quantity must be greater than zero.");

                var item = await _context.Items.FindAsync(dto.ItemId);
                if (item == null)
                    return Fail<ItemPurchaseResponseDto>("Selected item was not found in the catalog.");

                var purchase = _mapper.Map<ItemPurchase>(dto);
                purchase.TotalAmount = dto.Quantity * dto.UnitPrice;
                purchase.Status = ItemTransactionStatus.Pending;

                await _genericRepo.AddAsync(purchase);
                await _context.SaveChangesAsync();

                var result = await LoadResponseAsync(purchase.ItemPurchaseId);
                return Ok(result!, "Purchase recorded successfully and is pending approval.");
            }
            catch (Exception ex)
            {
                return Fail<ItemPurchaseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemPurchaseResponseDto>> UpdateAsync(string id, ItemPurchaseUpdateDto dto)
        {
            try
            {
                var purchase = await _genericRepo.GetByIdAsync(id);
                if (purchase == null)
                    return Fail<ItemPurchaseResponseDto>("Purchase record not found.");

                if ((dto.Quantity.HasValue || dto.UnitPrice.HasValue) && purchase.Status != ItemTransactionStatus.Pending)
                    return Fail<ItemPurchaseResponseDto>("Quantity/price can only be edited while the purchase is still Pending.");

                if (dto.Quantity.HasValue) purchase.Quantity = dto.Quantity.Value;
                if (dto.UnitPrice.HasValue) purchase.UnitPrice = dto.UnitPrice.Value;
                if (dto.Quantity.HasValue || dto.UnitPrice.HasValue)
                    purchase.TotalAmount = purchase.Quantity * purchase.UnitPrice;

                if (dto.PurchaseDate.HasValue) purchase.PurchaseDate = dto.PurchaseDate.Value;
                if (dto.SupplierName != null) purchase.SupplierName = dto.SupplierName;
                if (dto.InvoiceNumber != null) purchase.InvoiceNumber = dto.InvoiceNumber;
                if (dto.PaymentMode.HasValue) purchase.PaymentMode = dto.PaymentMode.Value;
                if (dto.VehicleId != null) purchase.VehicleId = dto.VehicleId;
                if (dto.Notes != null) purchase.Notes = dto.Notes;
                if (dto.ApprovedBy != null) purchase.ApprovedBy = dto.ApprovedBy;
                if (dto.Status.HasValue) purchase.Status = dto.Status.Value;

                await _genericRepo.Update(purchase);
                await _context.SaveChangesAsync();

                var result = await LoadResponseAsync(id);
                return Ok(result!, "Purchase updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemPurchaseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemPurchaseResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var result = await LoadResponseAsync(id);
                if (result == null)
                    return Fail<ItemPurchaseResponseDto>("Purchase record not found.");

                return Ok(result, "Purchase fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemPurchaseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetAllAsync()
        {
            try
            {
                var purchases = await _context.ItemPurchases
                    .Include(x => x.Item)
                    .OrderByDescending(x => x.PurchaseDate)
                    .ToListAsync();
                var result = MapList(purchases);
                return Ok(result, $"{result.Count} purchase record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemPurchaseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var purchase = await _genericRepo.GetByIdAsync(id);
                if (purchase == null)
                    return Fail<bool>("Purchase record not found.");

                // If stock was already added (Approved/Paid), reverse it before deleting.
                if (purchase.Status == ItemTransactionStatus.Approved || purchase.Status == ItemTransactionStatus.Paid)
                {
                    var item = await _context.Items.FindAsync(purchase.ItemId);
                    if (item != null) item.CurrentStock -= purchase.Quantity;
                }

                await _genericRepo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "Purchase deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetByItemAsync(string itemId)
        {
            try
            {
                var purchases = await _purchaseRepo.GetByItemAsync(itemId);
                var result = MapList(purchases);
                return Ok(result, $"{result.Count} purchase record(s) found for item.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemPurchaseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetByVehicleAsync(string vehicleId)
        {
            try
            {
                var purchases = await _purchaseRepo.GetByVehicleAsync(vehicleId);
                var result = MapList(purchases);
                return Ok(result, $"{result.Count} purchase record(s) found for vehicle.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemPurchaseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetByStatusAsync(ItemTransactionStatus status)
        {
            try
            {
                var purchases = await _purchaseRepo.GetByStatusAsync(status);
                var result = MapList(purchases);
                return Ok(result, $"{result.Count} purchase record(s) found with status {status}.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemPurchaseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                if (from > to)
                    return Fail<List<ItemPurchaseResponseDto>>("'From' date cannot be greater than 'To' date.");

                var purchases = await _purchaseRepo.GetByDateRangeAsync(from, to);
                var result = MapList(purchases);
                return Ok(result, $"{result.Count} purchase record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<ItemPurchaseResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemPurchaseResponseDto>> ApproveAsync(string id, string approvedBy)
        {
            try
            {
                var purchase = await _genericRepo.GetByIdAsync(id);
                if (purchase == null)
                    return Fail<ItemPurchaseResponseDto>("Purchase record not found.");

                if (purchase.Status == ItemTransactionStatus.Approved || purchase.Status == ItemTransactionStatus.Paid)
                    return Fail<ItemPurchaseResponseDto>("Purchase is already approved.");

                var item = await _context.Items.FindAsync(purchase.ItemId);
                if (item == null)
                    return Fail<ItemPurchaseResponseDto>("Related item no longer exists in the catalog.");

                purchase.Status = ItemTransactionStatus.Approved;
                purchase.ApprovedBy = approvedBy;
                item.CurrentStock += purchase.Quantity; // goods received into stock

                await _genericRepo.Update(purchase);
                await _context.SaveChangesAsync();

                var result = await LoadResponseAsync(id);
                return Ok(result!, "Purchase approved and stock updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemPurchaseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemPurchaseResponseDto>> RejectAsync(string id, string approvedBy)
        {
            try
            {
                var purchase = await _genericRepo.GetByIdAsync(id);
                if (purchase == null)
                    return Fail<ItemPurchaseResponseDto>("Purchase record not found.");

                if (purchase.Status == ItemTransactionStatus.Rejected)
                    return Fail<ItemPurchaseResponseDto>("Purchase is already rejected.");

                // If stock was already added (was Approved/Paid), reverse it.
                if (purchase.Status == ItemTransactionStatus.Approved || purchase.Status == ItemTransactionStatus.Paid)
                {
                    var item = await _context.Items.FindAsync(purchase.ItemId);
                    if (item != null) item.CurrentStock -= purchase.Quantity;
                }

                purchase.Status = ItemTransactionStatus.Rejected;
                purchase.ApprovedBy = approvedBy;

                await _genericRepo.Update(purchase);
                await _context.SaveChangesAsync();

                var result = await LoadResponseAsync(id);
                return Ok(result!, "Purchase rejected successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemPurchaseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemPurchaseResponseDto>> MarkPaidAsync(string id, string approvedBy)
        {
            try
            {
                var purchase = await _genericRepo.GetByIdAsync(id);
                if (purchase == null)
                    return Fail<ItemPurchaseResponseDto>("Purchase record not found.");

                if (purchase.Status != ItemTransactionStatus.Approved)
                    return Fail<ItemPurchaseResponseDto>("Only an approved purchase can be marked as paid.");

                purchase.Status = ItemTransactionStatus.Paid;
                purchase.ApprovedBy = approvedBy;

                await _genericRepo.Update(purchase);
                await _context.SaveChangesAsync();

                var result = await LoadResponseAsync(id);
                return Ok(result!, "Purchase marked as paid successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemPurchaseResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ItemPurchaseMonthlyReportDto>> GetMonthlyReportAsync(int year, int month)
        {
            try
            {
                var purchases = await _purchaseRepo.GetByMonthAsync(year, month);
                var total = await _purchaseRepo.GetTotalByMonthAsync(year, month);

                var report = new ItemPurchaseMonthlyReportDto
                {
                    Year = year,
                    Month = month,
                    TotalAmount = total,
                    TotalRecords = purchases.Count
                };

                return Ok(report, $"Monthly purchase report for {year}/{month} fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ItemPurchaseMonthlyReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ── Helpers ─────────────────────────────────────────────
        private async Task<ItemPurchaseResponseDto?> LoadResponseAsync(string id)
        {
            var purchase = await _context.ItemPurchases
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.ItemPurchaseId == id);

            return purchase == null ? null : MapOne(purchase);
        }

        private static ItemPurchaseResponseDto MapOne(ItemPurchase purchase) => new()
        {
            ItemPurchaseId = purchase.ItemPurchaseId,
            ItemId = purchase.ItemId,
            ItemName = purchase.Item?.ItemName ?? string.Empty,
            Quantity = purchase.Quantity,
            UnitPrice = purchase.UnitPrice,
            TotalAmount = purchase.TotalAmount,
            PurchaseDate = purchase.PurchaseDate,
            SupplierName = purchase.SupplierName,
            InvoiceNumber = purchase.InvoiceNumber,
            PaymentMode = purchase.PaymentMode,
            Status = purchase.Status,
            VehicleId = purchase.VehicleId,
            AddedBy = purchase.AddedBy,
            ApprovedBy = purchase.ApprovedBy,
            Notes = purchase.Notes,
            CreatedAt = purchase.CreatedAt,
        };

        private static List<ItemPurchaseResponseDto> MapList(List<ItemPurchase> purchases) =>
            purchases.Select(MapOne).ToList();
    }
}
