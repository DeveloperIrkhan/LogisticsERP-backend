namespace LogisticsERP.API.DTOs
{
    namespace LogisticsERP.API.DTOs.Dashboard
    {
        // ─── MAIN DASHBOARD SUMMARY ───────────────────────────────
        public class DashboardSummaryDto
        {
            public VehicleStatsDto VehicleStats { get; set; } = new();
            public FuelAnalyticsDto FuelAnalytics { get; set; } = new();
            public MaintenanceAnalyticsDto MaintenanceAnalytics { get; set; } = new();
            public ExpenseAnalyticsDto ExpenseAnalytics { get; set; } = new();
            public ExpiryAlertsResponseDto ExpiryAlerts { get; set; } = new();
            public DriverStatsDto DriverStatsDto { get; set; } = new();
        }

        // ─── VEHICLE STATS ────────────────────────────────────────
        public class VehicleStatsDto
        {
            public int TotalVehicles { get; set; }
            public int ActiveVehicles { get; set; }
            public int InactiveVehicles { get; set; }
            public int AssignedVehicles { get; set; }
            public int UnassignedVehicles { get; set; }
            public int TotalDrivers { get; set; }
            public int ActiveDrivers { get; set; }
            public int OnDutyDrivers { get; set; }
        }

        // ─── Drivers ALERTS ────────────────────────────────────────
        public class DriverStatsDto
        {
            public int TotalDrivers { get; set; }
            public int TotalActiveDrivers { get; set; }
            public int OnDutyDrivers { get; set; }
            public int InActiveDrivers { get; set; }
        }
        // ─── EXPIRY ALERTS ────────────────────────────────────────
        public class ExpiryAlertsResponseDto
        {
            public VehicleExpiryAlertDto VehicleExpiryAlerts { get; set; } = new();
            public DriverExpiryAlertsDto DriverExpiryAlerts { get; set; } = new();
        }
        public class VehicleExpiryAlertDto
        {
            public List<VehicleExpiryItemDto> ExpiredVehicles { get; set; } = new();
            public List<VehicleExpiryItemDto> VehicleExpiringIn30Days { get; set; } = new();
            public List<VehicleExpiryItemDto> VehicleExpiringIn60Days { get; set; } = new();
        }

        public class DriverExpiryAlertsDto
        {
            public List<DriverExpiryItemDto> ExpiredDrivers { get; set; } = new();
            public List<DriverExpiryItemDto> ExpiringDriverIn30Days { get; set; } = new();
            public List<DriverExpiryItemDto> ExpiringDriverIn60Days { get; set; } = new();

        }

        public class VehicleExpiryItemDto
        {
            public string VehicleId { get; set; }
            public string VehicleNumber { get; set; }
            public string ExpiryType { get; set; }  // Registration, Insurance, Fitness
            public DateTime ExpiryDate { get; set; }
            public int DaysRemaining { get; set; }
        }
        public class DriverExpiryItemDto
        {
            public string DriverId { get; set; }
            public string FullName { get; set; }
            public string MobileNumber { get; set; }
            public DateTime DateOfJoining { get; set; }
            public string ExpiryType { get; set; }  // cnic, licnese, Fitness
            public DateTime ExpiryDate { get; set; }
            public int DaysRemaining { get; set; }
        }

        // ─── FUEL ANALYTICS ───────────────────────────────────────
        public class FuelAnalyticsDto
        {
            public decimal TotalLitersThisMonth { get; set; }
            public decimal TotalCostThisMonth { get; set; }
            public decimal TotalLitersThisYear { get; set; }
            public decimal TotalCostThisYear { get; set; }
            public int TotalFuelRecords { get; set; }
            public List<MonthlyTrendDto> MonthlyTrend { get; set; } = new();
        }

        // ─── MAINTENANCE ANALYTICS ────────────────────────────────
        public class MaintenanceAnalyticsDto
        {
            public decimal TotalCostThisMonth { get; set; }
            public decimal TotalCostThisYear { get; set; }
            public int TotalRecordsThisMonth { get; set; }
            public int UpcomingMaintenanceCount { get; set; }
            public List<MonthlyTrendDto> MonthlyTrend { get; set; } = new();
        }

        // ─── EXPENSE ANALYTICS ────────────────────────────────────
        public class ExpenseAnalyticsDto
        {
            public decimal TotalThisMonth { get; set; }
            public decimal TotalThisYear { get; set; }
            public int PendingExpenses { get; set; }
            public int ApprovedExpenses { get; set; }
            public int RejectedExpenses { get; set; }
            public List<CategoryBreakdownDto> ByCategory { get; set; } = new();
        }

        // ─── SHARED ───────────────────────────────────────────────
        public class MonthlyTrendDto
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public decimal Amount { get; set; }
            public string MonthName => new DateTime(Year, Month, 1).ToString("MMM yyyy");
        }

        public class CategoryBreakdownDto
        {
            public string Category { get; set; } = string.Empty;
            public decimal Amount { get; set; }
            public int Count { get; set; }
        }
    }
}
