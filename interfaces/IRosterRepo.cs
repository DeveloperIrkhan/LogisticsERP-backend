using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IRosterRepo
    {
        // ── Roster ────────────────────────────────────────────
        Task<List<DutyRoster>> GetByMonthYearAsync(int month, int year);
        Task<DutyRoster?> GetWithEntriesAsync(string rosterId);

        // ── Entries ───────────────────────────────────────────
        Task<List<DutyRosterEntry>> GetEntriesByRosterAsync(string rosterId);
        Task<List<DutyRosterEntry>> GetEntriesByDateAsync(DateTime date);
        Task<List<DutyRosterEntry>> GetEntriesByDriverAsync(string driverId);
        Task<List<DutyRosterEntry>> GetEntriesByVehicleAsync(string vehicleId);
        Task<List<DutyRosterEntry>> GetEntriesByStatusAsync(RosterEntryStatus status);
        Task<List<DutyRosterEntry>> GetEntriesByDateRangeAsync(DateTime from, DateTime to);
        Task<bool> HasConflictAsync(string driverId, DateTime date, ShiftType shiftType, string? excludeEntryId = null);
        Task AddEntriesAsync(List<DutyRosterEntry> entries);
    }
}
