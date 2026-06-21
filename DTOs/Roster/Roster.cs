using LogisticsERP.API.enums;

namespace LogisticsERP.API.DTOs.Roster
{
    public class RosterCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public string? CreatedBy { get; set; }
        public string? Notes { get; set; }
    }

    // ─── UPDATE ROSTER ────────────────────────────────────────
    public class RosterUpdateDto
    {
        public string? Title { get; set; }
        public string? Notes { get; set; }
        public RosterStatus? Status { get; set; }
        public string? ApprovedBy { get; set; }
    }

    // ─── RESPONSE ROSTER ──────────────────────────────────────
    public class RosterResponseDto
    {
        public string RosterId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public RosterStatus Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? ApprovedBy { get; set; }
        public string? Notes { get; set; }
        public int TotalEntries { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── CREATE ROSTER ENTRY ──────────────────────────────────
    public class RosterEntryCreateDto
    {
        public string RosterId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public DateTime DutyDate { get; set; }
        public ShiftType ShiftType { get; set; }
        public TimeSpan ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public DutyType DutyType { get; set; } = DutyType.Routine;
        public string? Purpose { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public string? OfficerName { get; set; }
        public string? Donor { get; set; }
        public string? Notes { get; set; }
    }

    // ─── BULK CREATE ENTRIES (for monthly planning) ───────────
    public class RosterBulkCreateDto
    {
        public string RosterId { get; set; } = string.Empty;
        public List<RosterEntryCreateDto> Entries { get; set; } = new();
    }

    // ─── UPDATE ROSTER ENTRY ──────────────────────────────────
    public class RosterEntryUpdateDto
    {
        public string? DriverId { get; set; }
        public string? VehicleId { get; set; }
        public DateTime? DutyDate { get; set; }
        public ShiftType? ShiftType { get; set; }
        public TimeSpan? ShiftStart { get; set; }
        public TimeSpan? ShiftEnd { get; set; }
        public DutyType? DutyType { get; set; }
        public string? Purpose { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public string? OfficerName { get; set; }
        public string? Donor { get; set; }
        public string? Notes { get; set; }
        public RosterEntryStatus? Status { get; set; }
        public string? DutyLogId { get; set; }
    }

    // ─── RESPONSE ROSTER ENTRY ────────────────────────────────
    public class RosterEntryResponseDto
    {
        public string EntryId { get; set; } = string.Empty;
        public string RosterId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string VehicleNumber { get; set; } = string.Empty;
        public DateTime DutyDate { get; set; }
        public ShiftType ShiftType { get; set; }
        public TimeSpan ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public DutyType DutyType { get; set; }
        public string? Purpose { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public string? OfficerName { get; set; }
        public string? Donor { get; set; }
        public string? Notes { get; set; }
        public RosterEntryStatus Status { get; set; }
        public string? DutyLogId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── DAILY VIEW (duty logbook for a day) ─────────────────
    public class DailyRosterViewDto
    {
        public DateTime Date { get; set; }
        public int TotalAssigned { get; set; }
        public int Completed { get; set; }
        public int Missed { get; set; }
        public int InProgress { get; set; }
        public List<RosterEntryResponseDto> Shifts { get; set; } = new();
    }

    // ─── MONTHLY VIEW ─────────────────────────────────────────
    public class MonthlyRosterViewDto
    {
        public string RosterId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public RosterStatus Status { get; set; }
        public int TotalDuties { get; set; }
        public int CompletedDuties { get; set; }
        public int MissedDuties { get; set; }
        public int ScheduledDuties { get; set; }
        public List<DailyRosterViewDto> DailyView { get; set; } = new();
    }

    // ─── DRIVER ROSTER VIEW ───────────────────────────────────
    public class DriverRosterViewDto
    {
        public string DriverId { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public int TotalDuties { get; set; }
        public int CompletedDuties { get; set; }
        public int MissedDuties { get; set; }
        public List<RosterEntryResponseDto> Duties { get; set; } = new();
    }
}
