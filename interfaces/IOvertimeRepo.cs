using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IOvertimeRepo
    {
        Task<List<OvertimeDuty>> GetByDriverAsync(string driverId);
        Task<List<OvertimeDuty>> GetByDutyAsync(string dutyId);
        Task<List<OvertimeDuty>> GetByStatusAsync(OvertimeStatus status);
        Task<List<OvertimeDuty>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<List<OvertimeDuty>> GetByMonthAsync(int year, int month);
        Task<decimal> GetTotalHoursByDriverAsync(string driverId);
        Task<decimal> GetTotalAmountByDriverAsync(string driverId);
    }
}
