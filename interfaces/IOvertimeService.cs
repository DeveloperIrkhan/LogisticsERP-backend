using LogisticsERP.API.DTOs.Overtime;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IOvertimeService
    {
        // ── CRUD ──────────────────────────────────────────────
        Task<ApiResponse<OvertimeResponseDto>> CreateAsync(OvertimeCreateDto dto);
        Task<ApiResponse<OvertimeResponseDto>> UpdateAsync(string id, OvertimeUpdateDto dto);
        Task<ApiResponse<OvertimeResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<OvertimeResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(string id);

        // ── FILTERS ───────────────────────────────────────────
        Task<ApiResponse<List<OvertimeResponseDto>>> GetByDriverAsync(string driverId);
        Task<ApiResponse<List<OvertimeResponseDto>>> GetByDutyAsync(string dutyId);
        Task<ApiResponse<List<OvertimeResponseDto>>> GetByStatusAsync(OvertimeStatus status);
        Task<ApiResponse<List<OvertimeResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to);

        // ── APPROVAL ──────────────────────────────────────────
        Task<ApiResponse<OvertimeResponseDto>> ApproveAsync(string id, string approvedBy);
        Task<ApiResponse<OvertimeResponseDto>> RejectAsync(string id, string approvedBy);

        // ── REPORTS ───────────────────────────────────────────
        Task<ApiResponse<OvertimeReportDto>> GetMonthlyReportByDriverAsync(string driverId, int year, int month);
        Task<ApiResponse<OvertimeReportDto>> GetTotalByDriverAsync(string driverId);
    }
}
