using LogisticsERP.API.Data;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
    public class DutyLogRepo : IDutyRepo
       
    {
        private readonly AppDbContext _context;

        public DutyLogRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<List<DutyLogs>> GetActiveDutiesAsync()
        {
            return await _context.DutyLogs
                .AsNoTracking()
                .Where(x => x.Status == DutyStatus.InProgress)
                .OrderByDescending(x => x.DateOut)
                .ToListAsync();
        }

        public async Task<List<DutyLogs>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.DutyLogs
               .Where(x=> x.DateOut <= to && x.DateIn >= from)
               .OrderByDescending(x => x.DateOut)
               .ToListAsync();
        }

        public async Task<List<DutyLogs>> GetByDriverAsync(string driverId)
        {
            return await _context.DutyLogs
               .Where(x => x.DriverId == driverId)
               .OrderByDescending(x => x.DateOut)
               .ToListAsync();
        }

        public async Task<List<DutyLogs>> GetByStatusAsync(DutyStatus status)
        {
            return await _context.DutyLogs
               .Where(x => x.Status == status)
               .OrderByDescending(x => x.DateOut)
               .ToListAsync();
        }

        public async Task<List<DutyLogs>> GetByVehicleAsync(string vehicleId)
        {
            return await _context.DutyLogs
                .Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(x => x.DateOut)
                .ToListAsync();
        }
    }
}
