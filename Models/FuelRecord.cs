using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class FuelRecord
    {
        [Key]
        public string FuelId { get; set; } = $"PRCS-FUEL-{Guid.NewGuid()}";

        public string VehicleId { get; set; } = string.Empty;
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; } = new();

        public string DriverId { get; set; } = string.Empty;
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }

        public DateTime FuelingDate { get; set; }
        public int OdoMeterReading { get; set; }
        public decimal Liters { get; set; }
        public decimal CostPerLiter { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsFullTank { get; set; }
        public int? Mileage { get; set; }

        public string StationName { get; set; } = string.Empty;
        public string? StationLocation { get; set; }
        public string? Province { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? FuelType { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Donor { get; set; }
        public string? AddedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
