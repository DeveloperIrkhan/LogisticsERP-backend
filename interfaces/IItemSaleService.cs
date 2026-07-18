using LogisticsERP.API.DTOs.Item;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IItemSaleService
    {
        // ── CRUD ──────────────────────────────────────────────
        Task<ApiResponse<ItemSaleResponseDto>> CreateAsync(ItemSaleCreateDto dto);
        Task<ApiResponse<ItemSaleResponseDto>> UpdateAsync(string id, ItemSaleUpdateDto dto);
        Task<ApiResponse<ItemSaleResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<ItemSaleResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(string id);

        // ── FILTERS ───────────────────────────────────────────
        Task<ApiResponse<List<ItemSaleResponseDto>>> GetByItemAsync(string itemId);
        Task<ApiResponse<List<ItemSaleResponseDto>>> GetByVehicleAsync(string vehicleId);
        Task<ApiResponse<List<ItemSaleResponseDto>>> GetByStatusAsync(ItemTransactionStatus status);
        Task<ApiResponse<List<ItemSaleResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to);

        // ── APPROVAL ──────────────────────────────────────────
        Task<ApiResponse<ItemSaleResponseDto>> ApproveAsync(string id, string approvedBy);
        Task<ApiResponse<ItemSaleResponseDto>> RejectAsync(string id, string approvedBy);
        Task<ApiResponse<ItemSaleResponseDto>> MarkPaidAsync(string id, string approvedBy);

        // ── REPORTS ───────────────────────────────────────────
        Task<ApiResponse<ItemSaleMonthlyReportDto>> GetMonthlyReportAsync(int year, int month);

    }
}
