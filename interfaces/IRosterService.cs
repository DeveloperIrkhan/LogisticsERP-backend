using LogisticsERP.API.DTOs.Roster;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IRosterService
    {
        // ── ROSTER CRUD ───────────────────────────────────────
        Task<ApiResponse<RosterResponseDto>> CreateRosterAsync(RosterCreateDto dto);
        Task<ApiResponse<RosterResponseDto>> UpdateRosterAsync(string id, RosterUpdateDto dto);
        Task<ApiResponse<RosterResponseDto>> GetRosterByIdAsync(string id);
        Task<ApiResponse<List<RosterResponseDto>>> GetAllRostersAsync();
        Task<ApiResponse<bool>> DeleteRosterAsync(string id);

        // ── ROSTER STATUS ─────────────────────────────────────
        Task<ApiResponse<RosterResponseDto>> PublishRosterAsync(string id);
        Task<ApiResponse<RosterResponseDto>> ApproveRosterAsync(string id, string approvedBy);
        Task<ApiResponse<RosterResponseDto>> CloseRosterAsync(string id);

        // ── ENTRY CRUD ────────────────────────────────────────
        Task<ApiResponse<RosterEntryResponseDto>> CreateEntryAsync(RosterEntryCreateDto dto);
        Task<ApiResponse<List<RosterEntryResponseDto>>> BulkCreateEntriesAsync(RosterBulkCreateDto dto);
        Task<ApiResponse<RosterEntryResponseDto>> UpdateEntryAsync(string id, RosterEntryUpdateDto dto);
        Task<ApiResponse<bool>> DeleteEntryAsync(string id);

        // ── ENTRY STATUS ──────────────────────────────────────
        Task<ApiResponse<RosterEntryResponseDto>> MarkAsInProgressAsync(string entryId);
        Task<ApiResponse<RosterEntryResponseDto>> MarkAsCompletedAsync(string entryId, string dutyLogId);
        Task<ApiResponse<RosterEntryResponseDto>> MarkAsMissedAsync(string entryId);
        Task<ApiResponse<RosterEntryResponseDto>> CancelEntryAsync(string entryId, string reason);

        // ── VIEWS ─────────────────────────────────────────────
        Task<ApiResponse<MonthlyRosterViewDto>> GetMonthlyViewAsync(int month, int year);
        Task<ApiResponse<DailyRosterViewDto>> GetDailyViewAsync(DateTime date);
        Task<ApiResponse<DriverRosterViewDto>> GetDriverRosterViewAsync(string driverId, int month, int year);
    }
}
