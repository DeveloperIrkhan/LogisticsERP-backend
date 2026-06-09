using LogisticsERP.API.DTOs.Maintenance;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
        public interface IMaintenanceRepo
        {
        Task<List<MaintenanceRecord>> GetByVehicle(string vehicleId);
        Task<List<MaintenanceRecord>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<List<MaintenanceRecord>> GetUpcomingAsync(int days);
        Task<decimal> GetTotalCostByVehicleAsync(string vehicleId);
        Task<decimal> GetMonthlyCostAsync(int year, int month);
        Task<decimal> GetYearlyCostAsync(int year);
    }
}
