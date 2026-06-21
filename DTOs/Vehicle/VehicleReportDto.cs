namespace LogisticsERP.API.DTOs.Vehicle
{
    namespace LogisticsERP.API.DTOs.Reports
    {
        public class VehicleReportDto
        {
            public string VehicleId { get; set; } = string.Empty;
            public string VehicleNumber { get; set; } = string.Empty;
            public string ModelName { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public DateTime RegistrationExpiry { get; set; }
            public DateTime InsuranceExpiry { get; set; }
            public DateTime FitnessExpiry { get; set; }
            public decimal TotalFuelCost { get; set; }
            public decimal TotalMaintenanceCost { get; set; }
            public decimal TotalExpenseCost { get; set; }
            public decimal TotalCost { get; set; }
        }

        public class FuelReportDto
        {
            public string FuelId { get; set; } = string.Empty;
            public string VehicleNumber { get; set; } = string.Empty;
            public string DriverName { get; set; } = string.Empty;
            public DateTime FuelingDate { get; set; }
            public decimal Liters { get; set; }
            public decimal CostPerLiter { get; set; }
            public decimal TotalCost { get; set; }
            public string StationName { get; set; } = string.Empty;
            public string? FuelType { get; set; }
            public int OdoMeterReading { get; set; }
        }

        public class MaintenanceReportDto
        {
            public string MaintenanceRecordId { get; set; } = string.Empty;
            public string VehicleNumber { get; set; } = string.Empty;
            public DateTime MaintenanceDate { get; set; }
            public string? MaintenanceType { get; set; }
            public string? WorkshopName { get; set; }
            public decimal Cost { get; set; }
            public decimal CurrentKm { get; set; }
            public string? ChangedParts { get; set; }
            public string? InvoiceNumber { get; set; }
            public DateTime? NextMaintenanceDate { get; set; }
        }

        public class ExpenseReportDto
        {
            public string ExpenseId { get; set; } = string.Empty;
            public string ExpenseName { get; set; } = string.Empty;
            public decimal Amount { get; set; }
            public DateTime ExpenseDate { get; set; }
            public string Category { get; set; } = string.Empty;
            public string PaymentMode { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string? VehicleNumber { get; set; }
            public string? ReceiptNumber { get; set; }
            public string? ApprovedBy { get; set; }
        }

        public class ReportFilterDto
        {
            public DateTime? From { get; set; }
            public DateTime? To { get; set; }
            public string? VehicleId { get; set; }
            public string? DriverId { get; set; }
            public int? Year { get; set; }
            public int? Month { get; set; }
        }
    }
}
