using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    [Table("OvertimeDuties")]
    public class OvertimeDuty
    {
        [Key] 
        public string OvertimeDutyId { get; set; } = $"PRCS-OTD-{Guid.NewGuid()}";
        
        public string DriverId { get; set; } = string.Empty;
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }

        [ForeignKey("DutyId")]
        public string? DutyId { get; set; }
        public DutyLogs? DutyLog { get; set; }
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public decimal? RatePerHour { get; set; }
        public decimal? TotalAmount { get; set; }

        public string? Reason { get; set; }
        public string? Notes { get; set; }

        public string ApprovedBy { get; set; }
        [ForeignKey("UserId")]
        public User ApprovedByUser { get; set; }
        public OvertimeStatus Status { get; set; } = OvertimeStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
