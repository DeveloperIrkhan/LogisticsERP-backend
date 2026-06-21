using LogisticsERP.API.DTOs.Expense;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IExpenseService
    {
        // ── CRUD ──────────────────────────────────────────────
        Task<ApiResponse<ExpenseResponseDto>> CreateAsync(ExpenseCreateDto dto);
        Task<ApiResponse<ExpenseResponseDto>> UpdateAsync(string id, ExpenseUpdateDto dto);
        Task<ApiResponse<ExpenseResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<ExpenseResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(string id);

        // ── FILTERS ───────────────────────────────────────────
        Task<ApiResponse<List<ExpenseResponseDto>>> GetByVehicleAsync(string vehicleId);
        Task<ApiResponse<List<ExpenseResponseDto>>> GetByUserAsync(string userId);
        Task<ApiResponse<List<ExpenseResponseDto>>> GetByStatusAsync(ExpenseStatus status);
        Task<ApiResponse<List<ExpenseResponseDto>>> GetByCategoryAsync(ExpenseCategory category);
        Task<ApiResponse<List<ExpenseResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to);

        // ── APPROVAL ──────────────────────────────────────────
        Task<ApiResponse<ExpenseResponseDto>> ApproveAsync(string id, string approvedBy);
        Task<ApiResponse<ExpenseResponseDto>> RejectAsync(string id, string approvedBy);

        // ── REPORTS ───────────────────────────────────────────
        Task<ApiResponse<ExpenseMonthlyReportDto>> GetMonthlyReportAsync(int year, int month);
        Task<ApiResponse<List<ExpenseCategoryReportDto>>> GetCategoryReportAsync(int year, int month);
        Task<ApiResponse<ExpenseMonthlyReportDto>> GetYearlyReportAsync(int year);
    }
}
