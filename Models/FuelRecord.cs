using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class FuelRecord
    {
        [Key]
        public string FuelId { get; set; } = $"PRCS-FUEL-{Guid.NewGuid()}";

        //FORIGN KEY TABLE 
        // Navigation property for the related Vehicle
        // Not nullable, required assignment
        [ForeignKey("VehicleId")]public string VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
       [ForeignKey("DriverId")] public string DriverId { get; set; }
        public Driver Driver { get; set; }
        public DateTime FuelingDate { get; set; }
        public int OdoMeterReading { get; set; }
        public decimal Liters { get; set; }
        public decimal CostPerLiter { get; set; }
        public int? Mileage { get; set; }
        public decimal TotalCost { get; set; }  
        public decimal StationName { get; set; }
        public string StationLocation { get; set; }
        public string ReceiptNumber { get; set; }   
        public string Donor { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
