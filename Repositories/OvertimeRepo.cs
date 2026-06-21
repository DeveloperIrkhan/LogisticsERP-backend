using LogisticsERP.API.Data;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;


namespace LogisticsERP.API.Repositories
{
    public class OvertimeRepo : IOvertimeRepo
    {
        private readonly AppDbContext _context;

        public OvertimeRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OvertimeDuty>> GetByDriverAsync(string driverId)
        {
            return await _context.OvertimeDuties
                .Where(x => x.DriverId == driverId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<OvertimeDuty>> GetByDutyAsync(string dutyId)
        {
            return await _context.OvertimeDuties
                .Where(x => x.DutyId == dutyId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<OvertimeDuty>> GetByStatusAsync(OvertimeStatus status)
        {
            return await _context.OvertimeDuties
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<OvertimeDuty>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.OvertimeDuties
                .Where(x => x.Date >= from && x.Date <= to)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<OvertimeDuty>> GetByMonthAsync(int year, int month)
        {
            return await _context.OvertimeDuties
                .Where(x => x.Date.Year == year && x.Date.Month == month)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalHoursByDriverAsync(string driverId)
        {
            return await _context.OvertimeDuties
                .Where(x => x.DriverId == driverId && x.Status == OvertimeStatus.Approved)
                .SumAsync(x => x.Hours);
        }

        public async Task<decimal> GetTotalAmountByDriverAsync(string driverId)
        {
            return await _context.OvertimeDuties
                .Where(x => x.DriverId == driverId && x.Status == OvertimeStatus.Approved)
                .SumAsync(x => x.TotalAmount ?? 0);
        }
    }
}
