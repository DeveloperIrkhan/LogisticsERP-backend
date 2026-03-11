using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class DutyLogs
    {
        [Key] 
        public string DutyId { get; set; } = $"PRCS-DUTY-{Guid.NewGuid()}";
        [ForeignKey("VehicleId")]
        public string VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        [ForeignKey("DriverId")]
        public string DriverId { get; set; }
        public Driver Driver { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime DateOut { get; set; }
        public DateTime? DateIn { get; set; }
        public string Donor { get; set; }
        public decimal? KillometerOut { get; set; }
        public decimal? KillometerIn { get; set; }
        public decimal? TotalHours { get; set; }
        public string Purpose { get; set; }
        public string OfficerName { get; set; }

        [ForeignKey("UserId")]
        public int? ApprovedBy { get; set; }
        public User ApprovedByUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
