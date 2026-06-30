namespace LogisticsERP.API.DTOs.Fuel
{
    // ─── CREATE ───────────────────────────────────────────────
    public class FuelCreateDto
    {
        public string VehicleId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;
        public string? AddedBy { get; set; }
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
        public string? Notes { get; set; }
    }

    // ─── UPDATE ───────────────────────────────────────────────
    public class FuelUpdateDto
    {
        public string? DriverId { get; set; }
        public DateTime? FuelingDate { get; set; }
        public int? OdoMeterReading { get; set; }
        public decimal? Liters { get; set; }
        public decimal? CostPerLiter { get; set; }
        public decimal? TotalCost { get; set; }
        public bool? IsFullTank { get; set; }
        public int? Mileage { get; set; }
        public string? StationName { get; set; }
        public string? StationLocation { get; set; }
        public string? Province { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? FuelType { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Donor { get; set; }
        public string? Notes { get; set; }
    }

    // ─── RESPONSE ─────────────────────────────────────────────
    public class FuelResponseDto
    {
        public string FuelId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string DriverId { get; set; } = string.Empty;
        public string? AddedBy { get; set; }

        public VehicleSummaryDto? Vehicle { get; set; }
        public DriverSummaryDto? Driver { get; set; }

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
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── COST REPORT ──────────────────────────────────────────
    public class FuelCostReportDto
    {
        public int Year { get; set; }
        public int? Month { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalLiters { get; set; }
        public int TotalRecords { get; set; }
    }

    // ─── CONSUMPTION REPORT (per vehicle) ─────────────────────
    public class FuelConsumptionReportDto
    {
        public string VehicleId { get; set; } = string.Empty;
        public decimal TotalLiters { get; set; }
        public decimal TotalCost { get; set; }
        public int? AverageMileage { get; set; }
        public int TotalRecords { get; set; }
        public DateTime? LastFuelingDate { get; set; }
    }



    public class VehicleSummaryDto
    {
        public string VehicleId { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
    }

    public class DriverSummaryDto
    {
        public string DriverId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
