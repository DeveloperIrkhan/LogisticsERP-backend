using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;

namespace LogisticsERP.API.DTOs.DutyLogs
{
    public class DutyCreateDto
    {
        public string VehicleId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;
        public string FromLocation { get; set; } = string.Empty;
        public string ToLocation { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string OfficerName { get; set; } = string.Empty;
        public DateTime DateOut { get; set; }
        public DutyType DutyType { get; set; } = DutyType.Routine;
        public decimal? KillometerOut { get; set; }
        public string? Donor { get; set; }
        public string? Remarks { get; set; }
    }

    public class DutyUpdateDto
    {
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public string? Purpose { get; set; }
        public string? OfficerName { get; set; }
        public string? Donor { get; set; }
        public string? Remarks { get; set; }
        public DutyType? DutyType { get; set; }
        public DutyStatus? Status { get; set; }
        public DateTime? DateIn { get; set; }
        public decimal? KillometerOut { get; set; }
        public decimal? KillometerIn { get; set; }
        public decimal? TotalKm { get; set; }
        public decimal? TotalHours { get; set; }
    }

    public class EndDutyDto
    {
        public DateTime DateIn { get; set; }
        public decimal KillometerIn { get; set; }
        public string? Remarks { get; set; }
    }

    public class DutyResponseDto
    {
        public string DutyId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public VehicleResponseDto? Vehicle { get; set; }
        public string DriverId { get; set; } = string.Empty;
        public DriverResponseDto? Driver { get; set; }
        public string FromLocation { get; set; } = string.Empty;
        public string ToLocation { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string OfficerName { get; set; } = string.Empty;
        public string Donor { get; set; } = string.Empty;
        public DutyType DutyType { get; set; }
        public DutyStatus Status { get; set; }
        public DateTime DateOut { get; set; }
        public DateTime? DateIn { get; set; }
        public decimal? KillometerOut { get; set; }
        public decimal? KillometerIn { get; set; }
        public decimal? TotalKm { get; set; }
        public decimal? TotalHours { get; set; }
        public string? Remarks { get; set; }
        public string? ApprovedBy { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
