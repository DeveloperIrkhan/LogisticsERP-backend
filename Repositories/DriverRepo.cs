using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
    public class DriverRepo : IDriverRepo
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public DriverRepo(AppDbContext appDbContext, IMapper mapper)
        {
            _dbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<List<Driver>> GerDriversListForSpecficVehicle(string vehicleId)
        {
            var result = await _dbContext.Drivers.Where(x => x.VehicleId == vehicleId).ToListAsync();
            return result;
        }

        public async Task<List<Driver>> getAllDriverAsync()
        {
            return await _dbContext.Drivers
                .OrderByDescending(x=> x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Driver>> GetAvailableDriversAsync()
        {
            var avalibledriverList = await _dbContext.DutyLogs.Where(x=> x.Status == DutyStatus.InProgress).Select(x=> x.DriverId).ToListAsync();

            return await _dbContext.Drivers
                .Where(x => x.Status == DriverStatus.ACTIVE
                && !avalibledriverList.Contains(x.DriverId))
                .ToListAsync();
        }

        public async Task<List<Driver>> GetDriversByStatusAsync(DriverStatus status)
        {
            return await _dbContext.Drivers
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> IsCNICDuplicateAsync(string cnic, string? excludeDriverId = null)
        {
            var query = _dbContext.Drivers.Where(x => x.CNIC == cnic);
            if (excludeDriverId != null)
                query = query.Where(x => x.DriverId != excludeDriverId);
            return await query.AnyAsync();
        }

        public async Task<bool> IsLicenseDuplicateAsync(string licenseNumber, string? excludeDriverId = null)
        {
            var query = _dbContext.Drivers.Where(x => x.LicenseNumber == licenseNumber);
            if (excludeDriverId != null)
                query = query.Where(x => x.DriverId != excludeDriverId);
            return await query.AnyAsync();
        }
    } 
}
