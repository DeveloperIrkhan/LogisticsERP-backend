using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class MaintenanceRecord
    {
        [Key]
        public string MaintenanceRecordId { get; set; } = $"PRCS-MEN-{Guid.NewGuid()}";
        [ForeignKey("VehicleId")]
        public string VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        //ServiceDate
        public DateTime MaintenanceDate { get; set; }
        public decimal CurrentKm { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }

        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime NextServiceDate { get; set; }
        [ForeignKey("UserId")]
        public string AddedBy { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}



//var vehicleKm = vehicle.CurrentKm;

//var upcomingMaintenance = _context.MaintenanceRecords
//.Where(x => x.VehicleId == vehicle.VehicleId &&
//           (x.NextMaintenanceKm <= vehicleKm + 500 ||
//            x.NextMaintenanceDate <= DateTime.Today.AddDays(7)))
//.ToList();
