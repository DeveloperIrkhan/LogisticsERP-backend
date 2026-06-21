using LogisticsERP.API.DTOs.DutyLogs;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IDutyLogService
    {
        //----CRUD---------------------------------
        Task<ApiResponse<DutyResponseDto>> CreateAsync(DutyCreateDto dto);
        Task<ApiResponse<DutyResponseDto>> UpdateAsync(string Id,DutyUpdateDto dto);
        Task<ApiResponse<DutyResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<DutyResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(string id);


        //----Filtering--------------------------------
        Task<ApiResponse<List<DutyResponseDto>>> GetByVehicleAsync(string vehicleId);
        Task<ApiResponse<List<DutyResponseDto>>> GetByDriverAsync(string driverId);
        Task<ApiResponse<List<DutyResponseDto>>> GetByStatusAsync(DutyStatus status);
        Task<ApiResponse<List<DutyResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<ApiResponse<List<DutyResponseDto>>> GetActiveDutiesAsync();

        // ── DUTY TRACKING ─────────────────────────────────────
        Task<ApiResponse<DutyResponseDto>> StartDutyAsync(string dutyId);
        Task<ApiResponse<DutyResponseDto>> EndDutyAsync(string dutyId, EndDutyDto dto);
        Task<ApiResponse<DutyResponseDto>> ApproveDutyAsync(string dutyId, string approvedBy);
        public Task<ApiResponse<DutyResponseDto>> CancelDutyAsync(string dutyId, string reason);

    }
}
