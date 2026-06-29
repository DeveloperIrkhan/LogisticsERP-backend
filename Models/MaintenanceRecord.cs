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
        public decimal CurrentKm { get; set; }
        public string? MaintenanceType { get; set; }
        public string? WorkshopName { get; set; }
        public string? ChangedParts { get; set; }
        public string? InvoiceNumber { get; set; }


        public string Description { get; set; } = string.Empty;
        public decimal? NextMaintenanceKm { get; set; }
        public decimal Cost { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string? NextMaintenanceFor { get; set; }




        //relationships

        public string? AddedBy { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }


        public string VehicleId { get; set; } = string.Empty;
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; } = new();

        public string? DriverId { get; set; }   
        [ForeignKey("DriverId")]
        public Driver? Driver { get; set; }
    }
}



