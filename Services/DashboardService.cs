using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.LogisticsERP.API.DTOs.Dashboard;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class DashboardService : ServiceBaseFunctions, IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;

        }


        public async Task<ApiResponse<DashboardSummaryDto>> GetSummaryAsync()
        {
            try
            {
                var summary = new DashboardSummaryDto
                {
                    VehicleStats = (await GetVehicleStatsAsync()).Data!,
                    ExpiryAlerts = (await GetExpiryAlertsAsync()).Data!,
                    FuelAnalytics = (await GetFuelAnalyticsAsync()).Data!,
                    DriverStatsDto = (await GetDriverStateAsnyc()).Data!,
                    ExpenseAnalytics = (await GetExpenseAnalyticsAsync()).Data!,
                    MaintenanceAnalytics = (await GetMaintenanceAnalyticsAsync()).Data!
                };
                 return Ok(summary, "Dashboard summary fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DashboardSummaryDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        public async Task<ApiResponse<VehicleStatsDto>> GetVehicleStatsAsync()
        {
            try
            {
                var vehicles = await _context.Vehicles.ToListAsync();
                var drivers  = await _context.Drivers.ToListAsync();

                var vehiclesStateSummery = new VehicleStatsDto
                {
                    TotalDrivers = drivers.Count,
                    TotalVehicles = vehicles.Count,
                    ActiveDrivers = drivers.Count(x => x.Status == DriverStatus.Active),
                    ActiveVehicles = vehicles.Count(x => x.Status == VehicleStatus.Active),
                    AssignedVehicles = vehicles.Count(x => x.Drivers.Any(y => y.VehicleId == x.VehicleId)),
                    UnassignedVehicles = vehicles.Count(x => !drivers.Any(d => d.VehicleId == x.VehicleId)),
                    InactiveVehicles = vehicles.Count(x => x.Status == VehicleStatus.InActive),
                    OnDutyDrivers = await _context.DutyLogs.Where(x => x.Status == DutyStatus.InProgress)
                                                            .Select(x => x.DriverId)
                                                            .Distinct()
                                                            .CountAsync()
                };
                return Ok(vehiclesStateSummery, "Vehicle stats fetched successfully.");
            }
            catch(Exception ex)
            {
                return Fail<VehicleStatsDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DriverStatsDto>> GetDriverStateAsnyc()
        {
            try
            {
                var vehicles = await _context.Vehicles.ToListAsync();
                var drivers = await _context.Drivers.ToListAsync();
                var DriverStats = new DriverStatsDto
                {
                    TotalActiveDrivers = drivers.Count(x => x.Status == DriverStatus.Active),
                    InActiveDrivers = drivers.Count(x => x.Status == DriverStatus.Inactive),
                    OnDutyDrivers = await _context.DutyLogs.Where(x => x.Status == DutyStatus.InProgress)
                                                            .Select(x => x.DriverId)
                                                            .Distinct()
                                                            .CountAsync(),
                    TotalDrivers = drivers.Count
                };

                return Ok(DriverStats, "Drivers Stats fetched Successfully"); 
            }
            catch (Exception ex)
            {
                return Fail<DriverStatsDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        public async Task<ApiResponse<MaintenanceAnalyticsDto>> GetMaintenanceAnalyticsAsync()
        {
            try
            {
                var today = DateTime.UtcNow;
                var in30Days = today.AddDays(30);
                var maintenance = await _context.MaintenanceRecords.ToListAsync();


                var monthly = maintenance
                                .GroupBy(x => new { x.MaintenanceDate.Year, x.MaintenanceDate.Month })
                                .OrderBy(x => x.Key.Year).ThenBy(x => x.Key.Month)
                                .Select(x => new MonthlyTrendDto 
                                {
                                    Amount = x.Sum(x=> x.Cost),
                                    Month = x.Key.Month,
                                    Year = x.Key.Year
                                })
                                .ToList();

                var analytics = new MaintenanceAnalyticsDto
                {
                    TotalCostThisMonth = maintenance.Where(x => x.MaintenanceDate.Year == today.Year && x.MaintenanceDate.Month == today.Month).Sum(x => x.Cost),
                    TotalCostThisYear = maintenance.Where(x => x.MaintenanceDate.Year == today.Year).Sum(x => x.Cost),
                    TotalRecordsThisMonth = maintenance.Count(x => x.MaintenanceDate.Year == today.Year && x.MaintenanceDate.Month == today.Month),
                    UpcomingMaintenanceCount = maintenance.Count(x => x.NextMaintenanceDate.HasValue && x.NextMaintenanceDate >= today && x.NextMaintenanceDate <= in30Days),
                    MonthlyTrend = monthly
                };

                return Ok(analytics, "Maintenance analytics fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<MaintenanceAnalyticsDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        public async Task<ApiResponse<ExpenseAnalyticsDto>> GetExpenseAnalyticsAsync()
        {
            try
            {
                var today = DateTime.UtcNow;
                var expenses = await _context.Expenses.ToListAsync();

                var byCategory = expenses
                    .GroupBy(x => x.ExpenseCategory.ToString())
                    .Select(g => new CategoryBreakdownDto
                    {
                        Category = g.Key,
                        Amount = g.Sum(x => x.Amount),
                        Count = g.Count()
                    }).ToList();

                var analytics = new ExpenseAnalyticsDto
                {
                    TotalThisMonth = expenses.Where(x => x.ExpenseDate.Year == today.Year && x.ExpenseDate.Month == today.Month).Sum(x => x.Amount),
                    TotalThisYear = expenses.Where(x => x.ExpenseDate.Year == today.Year).Sum(x => x.Amount),
                    PendingExpenses = expenses.Count(x => x.ExpenseStatus == ExpenseStatus.Pending),
                    ApprovedExpenses = expenses.Count(x => x.ExpenseStatus == ExpenseStatus.Approved),
                    RejectedExpenses = expenses.Count(x => x.ExpenseStatus == ExpenseStatus.Rejected),
                    ByCategory = byCategory
                };

                return Ok(analytics, "Expense analytics fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<ExpenseAnalyticsDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<ExpiryAlertsDto>> GetExpiryAlertsAsync()
        {
            try
            {
                var today = DateTime.UtcNow;
                var in30Days = today.AddDays(30);
                var in60Days = today.AddDays(60);
                var vehicles = await _context.Vehicles.ToListAsync();


                var alerts = new ExpiryAlertsDto();
                foreach (var vehicle in vehicles) 
                {
                    AddExpiryItem(alerts, vehicle.VehicleId, vehicle.Number, "Registeration", vehicle.RegistrationExpiry,today, in30Days, in60Days);
                    AddExpiryItem(alerts, vehicle.VehicleId, vehicle.Number, "Insurance", vehicle.InsuranceExpiry, today, in30Days, in60Days);
                    AddExpiryItem(alerts, vehicle.VehicleId, vehicle.Number, "Fittness", vehicle.FitnessExpiry,today, in30Days, in60Days);
                }
                return Ok(alerts, "Expiry alerts fetched successfully.");
            }
            catch(Exception exp)
            {
                return Fail<ExpiryAlertsDto>(exp.InnerException?.Message ?? exp.Message);
            }
        }

        public async Task<ApiResponse<FuelAnalyticsDto>> GetFuelAnalyticsAsync()
        {
            try
            {
                var today = DateTime.UtcNow;
                var fuel = await _context.FuelRecords.ToListAsync();

                var monthly = fuel
                    .GroupBy(x => new { x.FuelingDate.Year, x.FuelingDate.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .Select(g => new MonthlyTrendDto
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Amount = g.Sum(x => x.TotalCost)
                    }).ToList();

                var analytics = new FuelAnalyticsDto
                {
                    TotalLitersThisMonth = fuel.Where(x => x.FuelingDate.Year == today.Year && x.FuelingDate.Month == today.Month).Sum(x => x.Liters),
                    TotalCostThisMonth = fuel.Where(x => x.FuelingDate.Year == today.Year && x.FuelingDate.Month == today.Month).Sum(x => x.TotalCost),
                    TotalLitersThisYear = fuel.Where(x => x.FuelingDate.Year == today.Year).Sum(x => x.Liters),
                    TotalCostThisYear = fuel.Where(x => x.FuelingDate.Year == today.Year).Sum(x => x.TotalCost),
                    TotalFuelRecords = fuel.Count,
                    MonthlyTrend = monthly
                };

                return Ok(analytics, "Fuel analytics fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<FuelAnalyticsDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        //Helper Function 
        private void AddExpiryItem(ExpiryAlertsDto alerts, string vehicleId, string number, string type, DateTime expiry, DateTime today, DateTime in30, DateTime in60)
        {
            var days = (expiry-today).Days;
            var item = new ExpiryItemDto
            {
                VehicleId = vehicleId,
                DaysRemaining = days,
                ExpiryDate = expiry,
                ExpiryType = type,
                VehicleNumber = number,
            };


            if (expiry < today)
                alerts.ExpiredVehicles.Add(item);
            else if (expiry <= in30)
                alerts.ExpiringIn30Days.Add(item);
            else if(expiry <= in60)
                alerts.ExpiringIn60Days.Add(item);
        }

    }
}
