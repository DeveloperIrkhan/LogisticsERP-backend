using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    [Table("OvertimeDuties")]
    public class OvertimeDuty
    {
        [Key] 
        public string OvertimeDutyId { get; set; } = $"PRCS-OTD-{Guid.NewGuid()}";
        
        [ForeignKey("DriverId")]
        public string DriverId { get; set; }
        public Driver Driver { get; set; }
        
        [ForeignKey("DutyId")]
        public string? DutyId { get; set; }
        public DutyLogs DutyLog { get; set; }
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal TotalAmount { get; set; }
        [ForeignKey("UserId")]
        public int ApprovedBy { get; set; }
        public User ApprovedByUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
