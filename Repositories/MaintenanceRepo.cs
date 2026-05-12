using LogisticsERP.API.Data;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.Repositories
{
    public class MaintenanceRepo : IMaintenanceRepo
    {
        private readonly AppDbContext _context;

        public MaintenanceRepo(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public Task<List<MaintenanceRecord>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<List<MaintenanceRecord>> GetByVehicleAsync(string vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<List<MaintenanceRecord>> GetUpcomingAsync(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
