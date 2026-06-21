using LogisticsERP.API.enums;

namespace LogisticsERP.API.DTOs.Overtime
{
    // ─── CREATE ───────────────────────────────────────────────
    public class OvertimeCreateDto
    {
        public string DriverId { get; set; } = string.Empty;
        public string? DutyId { get; set; }
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public decimal RatePerHour { get; set; }
        public string? Reason { get; set; }
        public string? Notes { get; set; }
    }

    // ─── UPDATE ───────────────────────────────────────────────
    public class OvertimeUpdateDto
    {
        public DateTime? Date { get; set; }
        public decimal? Hours { get; set; }
        public decimal? RatePerHour { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Reason { get; set; }
        public string? Notes { get; set; }
        public OvertimeStatus? Status { get; set; }
    }

    // ─── RESPONSE ─────────────────────────────────────────────
    public class OvertimeResponseDto
    {
        public string OvertimeDutyId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;
        public string? DutyId { get; set; }
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Reason { get; set; }
        public string? Notes { get; set; }
        public string? ApprovedBy { get; set; }
        public OvertimeStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── REPORT ───────────────────────────────────────────────
    public class OvertimeReportDto
    {
        public string DriverId { get; set; } = string.Empty;
        public decimal TotalHours { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalRecords { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
