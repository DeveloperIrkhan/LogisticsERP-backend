using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IDutyRepo
    {
        Task<List<DutyLogs>> GetByVehicleAsync(string vehicleId);
        Task<List<DutyLogs>> GetByDriverAsync(string driverId);
        Task<List<DutyLogs>> GetByStatusAsync(DutyStatus status);
        Task<List<DutyLogs>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<List<DutyLogs>> GetActiveDutiesAsync();
    }
}
