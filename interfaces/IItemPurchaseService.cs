using LogisticsERP.API.DTOs.Item;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IItemPurchaseService
    {
        // ── CRUD ──────────────────────────────────────────────
        Task<ApiResponse<ItemPurchaseResponseDto>> CreateAsync(ItemPurchaseCreateDto dto);
        Task<ApiResponse<ItemPurchaseResponseDto>> UpdateAsync(string id, ItemPurchaseUpdateDto dto);
        Task<ApiResponse<ItemPurchaseResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(string id);

        // ── FILTERS ───────────────────────────────────────────
        Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetByItemAsync(string itemId);
        Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetByVehicleAsync(string vehicleId);
        Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetByStatusAsync(ItemTransactionStatus status);
        Task<ApiResponse<List<ItemPurchaseResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to);

        // ── APPROVAL ──────────────────────────────────────────
        Task<ApiResponse<ItemPurchaseResponseDto>> ApproveAsync(string id, string approvedBy);
        Task<ApiResponse<ItemPurchaseResponseDto>> RejectAsync(string id, string approvedBy);
        Task<ApiResponse<ItemPurchaseResponseDto>> MarkPaidAsync(string id, string approvedBy);

        // ── REPORTS ───────────────────────────────────────────
        Task<ApiResponse<ItemPurchaseMonthlyReportDto>> GetMonthlyReportAsync(int year, int month);

    }
}
