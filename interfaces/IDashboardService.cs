using LogisticsERP.API.DTOs.LogisticsERP.API.DTOs.Dashboard;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IDashboardService
    {
        Task<ApiResponse<DashboardSummaryDto>> GetSummaryAsync();
        Task<ApiResponse<VehicleStatsDto>> GetVehicleStatsAsync();
        Task<ApiResponse<ExpiryAlertsResponseDto>> GetExpiryAlertsAsync();
        Task<ApiResponse<FuelAnalyticsDto>> GetFuelAnalyticsAsync();
        Task<ApiResponse<MaintenanceAnalyticsDto>> GetMaintenanceAnalyticsAsync();
        Task<ApiResponse<ExpenseAnalyticsDto>> GetExpenseAnalyticsAsync();
        Task<ApiResponse<DriverStatsDto>> GetDriverStateAsnyc();


    }
}
