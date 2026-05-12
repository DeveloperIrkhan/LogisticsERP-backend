using LogisticsERP.API.DTOs.Maintenance;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
        public interface IMaintenanceRepo
        {
            //Task<MaintenanceRecord> CreateAsync(MaintenanceRecord entity);
            //Task<MaintenanceRecord> UpdateAsync(MaintenanceRecord entity);
            //Task<MaintenanceRecord?> GetByIdAsync(string id);
            //Task<List<MaintenanceRecord>> GetAllAsync();
            //Task<bool> DeleteAsync(string id);
            Task<List<MaintenanceRecord>> GetByVehicleAsync(string vehicleId);
            Task<List<MaintenanceRecord>> GetByDateRangeAsync(DateTime from, DateTime to);
            Task<List<MaintenanceRecord>> GetUpcomingAsync(DateTime from, DateTime to);
        }
}
