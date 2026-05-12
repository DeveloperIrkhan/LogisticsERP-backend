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

        public async Task<List<VehicleResponseDto>> GetAssignedVehicleList(VehicleStatus Status)
        {
            //if (!Enum.TryParse<VehicleStatus>(Status, true, out var parsedStatus))
            //    throw new Exception("Invalid status");

            var result = await _context.Vehicles
                .Include(x => x.Drivers)
                .Include(x => x.Documents)
                .Where(x => x.Status == Status)
                .ToListAsync();

            return _mapper.Map<List<VehicleResponseDto>>(result);
        }

        public async Task<List<VehicleResponseDto>> GetUnAssignedVehicleList(VehicleStatus Status)
        {

            var result = await _context.Vehicles
                .Include(x => x.Drivers)
                .Include(x => x.Documents)
                .Where(x => x.Status == Status)
                .ToListAsync();

            return _mapper.Map<List<VehicleResponseDto>>(result);
        }


        public async Task<List<VehicleResponseDto>> GetVehicles(VehicleFilterDto filter)
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

            if (!string.IsNullOrEmpty(filter.Type))
                query = query.Where(x => x.Type == filter.Type);

            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status);

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.RegistrationDate >= filter.ToDate);

            if (filter.FromDate.HasValue)
                query = query.Where(x => x.RegistrationDate <= filter.FromDate);

            var result = await query.ToListAsync();
            return _mapper.Map<List<VehicleResponseDto>>(result);

        }

        public async Task<VehicleResponseDto> GetVehicleById(string vehicleId)
        {
            var result = await _context.Vehicles
                .Include(x => x.Drivers)
                .Include(x => x.Documents)
                .FirstOrDefaultAsync(x => x.VehicleId == vehicleId);

            if (result == null)
                throw new Exception("Vehicle not found");
            return _mapper.Map<VehicleResponseDto>(result);
        }

        public async Task<bool> IsDriverAlreadyAssignedToAnotherVehicle(string vehicleId, string driverId)
        {
            return await _context.Drivers.AnyAsync(d => 
            d.DriverId == driverId && d.VehicleId == vehicleId);
        }

        public async Task<bool> IsDriverAlreadyAssignedToSameVehicle(string vehicleId, string driverId)
        {
            return await _context.Drivers.AnyAsync(d =>
           d.DriverId == driverId && d.VehicleId != null);
        }

        public async Task<bool> IsVehicleActive(string vehicleId)
        {
            return await _context.Vehicles.AnyAsync(v => v.VehicleId == vehicleId && v.Status == VehicleStatus.Active);
        }

        public async Task<ApiResponse<VehicleResponseDto>> ChangeStatusOfVehicle(string vehicleId, VehicleStatus status)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x=> x.VehicleId ==vehicleId);
            if (vehicle == null) return new ApiResponse<VehicleResponseDto>()
            {
                Message = "vehicle on giving ID not found",
                Data = null,
                Success = false
            };

            vehicle.Status = status;
            await _context.SaveChangesAsync();
            return new ApiResponse<VehicleResponseDto>
            {
                Success = true,
                Message = "Vehicle status updated successfully",
                Data = _mapper.Map<VehicleResponseDto>(vehicle)
            };

        }

        public async Task<ApiResponse<VehicleResponseDto>> GetDocumentOfVehicleById(string vehicleId)
        {
            var result = await _context.Vehicles
                .Include(x => x.Documents)
                .FirstOrDefaultAsync(x => x.VehicleId == vehicleId);

            if (result == null)
            {
                return new ApiResponse<VehicleResponseDto>
                {
                    Success = false,
                    Message = "Vehicle not found",
                    Data = null
                };

            }
            return new ApiResponse<VehicleResponseDto>
            {
                Success = true,
                Message = "Vehicle found",
                Data = _mapper.Map<VehicleResponseDto>(result)
            };
        }

        public async Task<IEnumerable<VehicleResponseDto>> GetVehicleStatusAsync(VehicleStatus vehicleStatus)
        {
            var result = await _context.Vehicles.Where(x => x.Status == vehicleStatus).ToListAsync();
            return _mapper.Map<IEnumerable<VehicleResponseDto>>(result);
        }
    }
}
