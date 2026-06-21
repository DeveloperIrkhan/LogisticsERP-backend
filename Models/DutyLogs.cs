using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class DutyLogs
    {
        [Key] 
        public string DutyId { get; set; } = $"PRCS-DUTY-{Guid.NewGuid()}";
        public string VehicleId { get; set; } = string.Empty;
        
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; } 
        public string DriverId { get; set; } = string.Empty;
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; } 
        public DutyType DutyType { get; set; } = DutyType.Routine;
        public DutyStatus Status { get; set; } 
        public string FromLocation { get; set; } = string.Empty;
        public string ToLocation { get; set; } = string.Empty;
        public DateTime DateOut { get; set; }
        public DateTime DateIn { get; set; }
        public string Donor { get; set; } = string.Empty;
        public decimal? KillometerOut { get; set; }
        public decimal? KillometerIn { get; set; }
        public decimal? TotalHours { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string OfficerName { get; set; } = string.Empty;
        public string? Remarks { get; set; } // any notes about the duty
        public decimal? TotalKm { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; } = DateTime.UtcNow;
        public string? ApprovedBy { get; set; }
        [ForeignKey("UserId")]
        public User? ApprovedByUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
