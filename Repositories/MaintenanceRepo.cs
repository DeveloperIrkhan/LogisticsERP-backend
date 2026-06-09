using LogisticsERP.API.Data;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
    public class MaintenanceRepo : IMaintenanceRepo
    {
        private readonly AppDbContext _context;

        public MaintenanceRepo(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<MaintenanceRecord>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.MaintenanceRecords
                 .Where(x => x.MaintenanceDate >= from && x.MaintenanceDate <= to)
                 .OrderByDescending(x => x.MaintenanceDate)
                 .ToListAsync();
        }

        public async Task<List<MaintenanceRecord>> GetByVehicle(string vehicleId)
        {
            return await _context.MaintenanceRecords
                .Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(x => x.MaintenanceDate)
                .ToListAsync();
        }

        public async Task<decimal> GetMonthlyCostAsync(int year, int month)
        {
            return await _context.MaintenanceRecords
                .Where(x => x.MaintenanceDate.Year == year &&
                x.MaintenanceDate.Month == month).SumAsync(x => x.Cost);
        }

        public async Task<decimal> GetTotalCostByVehicleAsync(string vehicleId)
        {
            return await _context.MaintenanceRecords
                .Where(x => x.VehicleId == vehicleId).SumAsync(x => x.Cost);
        }

        public async Task<List<MaintenanceRecord>> GetUpcomingAsync(int days)
        {
            var today = DateTime.Today;
            var target = today.AddDays(days);

            return await _context.MaintenanceRecords
                .Where(x => x.NextMaintenanceDate.HasValue
                && x.NextMaintenanceDate >= today 
                && x.NextMaintenanceDate <= target)
                .OrderBy(x=> x.MaintenanceDate)
                .ToListAsync();
        }

        public async Task<decimal> GetYearlyCostAsync(int year)
        {
            return await _context.MaintenanceRecords
                .Where(x => x.MaintenanceDate.Year == year)
                .SumAsync(x => x.Cost);
        }
    }
}
