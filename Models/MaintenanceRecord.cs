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
        //ServiceDate
        public decimal CurrentKm { get; set; }
        public string? MaintenanceType { get; set; }
        public string? WorkshopName { get; set; }
        public string? ChangedParts { get; set; }
        public string? InvoiceNumber { get; set; }


        public string Description { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public decimal Cost { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }



        //relationships

        [ForeignKey("UserId")]
        public string AddedBy { get; set; }
        public User User { get; set; }


        [ForeignKey("VehicleId")]
        public string VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}



//var vehicleKm = vehicle.CurrentKm;

//var upcomingMaintenance = _context.MaintenanceRecords
//.Where(x => x.VehicleId == vehicle.VehicleId &&
//           (x.NextMaintenanceKm <= vehicleKm + 500 ||
//            x.NextMaintenanceDate <= DateTime.Today.AddDays(7)))
//.ToList();
