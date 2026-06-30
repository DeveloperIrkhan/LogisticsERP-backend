    using LogisticsERP.API.Data;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
    public class FuelRepo : IFuelRepo
    {
        private readonly AppDbContext _context;

        public FuelRepo(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<FuelRecord>> GetAll()
        {
            return await _context.FuelRecords
                .Include(x => x.Driver)
                .Include(x=> x.Vehicle)
                .ToListAsync();
        }

        public async Task<FuelRecord> GetByIdAsync(string fuelId)
        {
            return await _context.FuelRecords
                .Where(x => x.FuelId == fuelId)
                 .Include(x => x.Driver)
                 .Include(x => x.Vehicle)
                 .FirstOrDefaultAsync();
        }

        public async Task<FuelRecord?> GetLastRecordByVehicleAsync(string vehicleId)
        {
            return await _context.FuelRecords
                         .Where(x => x.VehicleId == vehicleId)
                         .OrderByDescending(x => x.FuelingDate)
                         .FirstOrDefaultAsync();
        }

        public async Task<List<FuelRecord>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.FuelRecords
                .Where(x => x.FuelingDate >= from && x.FuelingDate <= to)
                .OrderByDescending(x => x.FuelingDate)
                .ToListAsync();
        }

        public async Task<List<FuelRecord>> GetByDriverAsync(string driverId)
        {
            return await _context.FuelRecords
                       .Where(x => x.DriverId == driverId)
                       .OrderByDescending(x => x.FuelingDate)
                       .ToListAsync();
        }

       
        public async Task<List<FuelRecord>> GetByVehicleAsync(string vehicleId)
        {
            return await _context.FuelRecords
                        .Where(x => x.VehicleId == vehicleId)
                        .OrderByDescending(x => x.FuelingDate)
                        .ToListAsync();
        }

      

        public async Task<decimal> GetMonthlyCostAsync(int year, int month)
        {
            return await _context.FuelRecords
                            .Where(x => x.FuelingDate.Year == year &&
                                   x.FuelingDate.Month == month)
                            .OrderByDescending(x => x.FuelingDate)
                            .SumAsync(x => x.TotalCost);
        }

        public async Task<decimal> GetMonthlyLitersAsync(int year, int month)
        {
            return await _context.FuelRecords
                           .Where(x => x.FuelingDate.Year == year &&
                                  x.FuelingDate.Month == month)
                           .OrderByDescending(x => x.FuelingDate)
                           .SumAsync(x => x.Liters);
        }

        public async Task<decimal> GetTotalCostByVehicleAsync(string vehicleId)
        {
            return await _context.FuelRecords
                            .Where(x => x.VehicleId == vehicleId)
                            .SumAsync(x => x.TotalCost);
        }

        public async Task<decimal> GetTotalLitersByVehicleAsync(string vehicleId)
        {
            return await _context.FuelRecords
                            .Where(x => x.VehicleId == vehicleId)
                            .SumAsync(x => x.Liters);
        }

        public async Task<decimal> GetYearlyCostAsync(int year)
        {
            return await _context.FuelRecords
                .Where(x => x.FuelingDate.Year == year)
                .SumAsync(x => x.TotalCost);
        }

        public async Task<decimal> GetYearlyLitersAsync(int year)
        {
            return await _context.FuelRecords
                .Where(x => x.FuelingDate.Year == year)
                .SumAsync(x => x.Liters);
        }
    }
}
