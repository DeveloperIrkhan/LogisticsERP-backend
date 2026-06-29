using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Net.NetworkInformation;

namespace LogisticsERP.API.Repositories
{
    public class VehicleRepo : IVehicleRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VehicleRepo(AppDbContext appDbContext, IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
           return await _context.Vehicles
                        .Include(x=> x.Drivers)
                        .Include(x=> x.Documents)
                        .ToListAsync();
        }

        public async Task<List<Vehicle>> GetAssignedVehicleList(VehicleStatus Status)
        {
            var result = await _context.Vehicles
                .Include(x => x.Drivers)
                .Include(x => x.Documents)
                .Where(x => x.Status == Status)
                .ToListAsync();

            return result;
        }

        public async Task<List<Vehicle>> GetUnAssignedVehicleList(VehicleStatus Status)
        {

            var result = await _context.Vehicles
                .Include(x => x.Drivers)
                .Include(x => x.Documents)
                .Where(x => x.Status == Status)
                .ToListAsync();

            return result;
        }


        public async Task<List<Vehicle>> GetVehicles(VehicleFilterDto filter)
        {

            var query = _context.Vehicles
                .Include(x => x.Documents)
                .Include(x => x.Drivers).AsQueryable();

            if (!string.IsNullOrEmpty(filter.DriverId))
            {
                query = query.Where(x => x.Drivers
                    .Any(d => d.DriverId.Contains(filter.DriverId)));
            }

            if (!string.IsNullOrEmpty(filter.Number))
                query = query.Where(x => x.Number.Contains(filter.Number));

            if (!string.IsNullOrEmpty(filter.Doner))
                query = query.Where(x => x.Doner.Contains(filter.Doner));

            if (!string.IsNullOrEmpty(filter.InsuredBy))
                query = query.Where(x => x.InsuredBy.Contains(filter.InsuredBy));


            if (!string.IsNullOrEmpty(filter.Company))
                query = query.Where(x => x.Company.Contains(filter.Company));

            if (!string.IsNullOrEmpty(filter.VehicleType))
                query = query.Where(x => x.VehicleType == filter.VehicleType);

            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status);

            if (filter.FromDate.HasValue)
                query = query.Where(x => x.RegistrationDate >= filter.FromDate);

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.RegistrationDate <= filter.ToDate);

            var result = await query.ToListAsync();
            return result;

        }

        public async Task<Vehicle> GetVehicleById(string vehicleId)
        {
            var result = await _context.Vehicles
                .Include(x => x.Drivers)
                .Include(x => x.Documents)
                .FirstOrDefaultAsync(x => x.VehicleId == vehicleId);

            if (result == null)
                throw new Exception("Vehicle not found");
            return result;
        }

        public async Task<bool> IsDriverAlreadyAssignedToAnotherVehicle(string vehicleId, string driverId)
        {
            return await _context.Drivers.AnyAsync(d => 
            d.DriverId == driverId && d.VehicleId != null && d.VehicleId != vehicleId);
        }

        public async Task<bool> IsDriverAlreadyAssignedToSameVehicle(string vehicleId, string driverId)
        {
            return await _context.Drivers.AnyAsync(d =>
           d.DriverId == driverId && d.VehicleId == null);
        }

        public async Task<bool> IsVehicleActive(string vehicleId)
        {
            return await _context.Vehicles.AnyAsync(v => v.VehicleId == vehicleId && v.Status == VehicleStatus.Active);
        }

        public async Task<Vehicle?> ChangeStatusOfVehicle(string vehicleId, VehicleStatus status)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x=> x.VehicleId ==vehicleId);
            vehicle?.Status = status;
            return vehicle;

        }
        public async Task<Vehicle?> GetDocumentOfVehicleById(string vehicleId)
        {
            return await _context.Vehicles
                .Where(x=> x.VehicleId == vehicleId)
                .Include(x => x.Documents)
                .OrderByDescending(x=> x.RegistrationDate)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Vehicle>> GetVehicleStatusAsync(VehicleStatus vehicleStatus)
        {
            var result = await _context.Vehicles.Where(x => x.Status == vehicleStatus).ToListAsync();
            return result;
        }

    }
}
