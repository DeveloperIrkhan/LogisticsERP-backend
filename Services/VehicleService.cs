using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IGenericRepo<Vehicle> _repo;
        private readonly IMapper _mapper;
        private readonly IVehicleRepo _vehicleRepo;
        private readonly AppDbContext _context;

        public VehicleService(IGenericRepo<Vehicle> genericRepo, IMapper mapper, AppDbContext appDbContext, IVehicleRepo vehicleRepo)
        {
            _repo = genericRepo;
            _mapper = mapper;
            _vehicleRepo = vehicleRepo;
            _context = appDbContext;
        }

        public async Task<List<VehicleResponseDto>> GetVehicles(VehicleFilterDto vehicleFilter)
        {
            return await _vehicleRepo.GetVehicles(vehicleFilter);
        }

        public async Task<VehicleResponseDto> CreateVehicle(VehicleCreateDto vehicle)
        {
            var newVehicle = _mapper.Map<Vehicle>(vehicle);
            await _repo.AddAsync(newVehicle);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<VehicleResponseDto>(newVehicle);
            return response;
        }

        public async Task DeleteVehicle(string id)
        {
            await _repo.Delete(id);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<VehicleResponseDto>> GetAllVehicles()
        {
            //var vehicles = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<VehicleResponseDto>>(await _repo.GetAllAsync());

        }

        public async Task<List<VehicleResponseDto>> GetExpiringVehicles(int days)
        {
            var today = DateTime.UtcNow;
            var nextDates = today.AddDays(days);
            var expiredVehiclesList = (await _repo.WhereAsync(
                v => v.RegistrationExpiry >= today && v.RegistrationExpiry <= nextDates
                ||
                v.FitnessExpiry >= today && v.FitnessExpiry <= nextDates
                ||
                v.InsuranceExpiry >= today && v.InsuranceExpiry <= nextDates
                )).OrderBy(v => v.RegistrationDate).ToList();
            return _mapper.Map<List<VehicleResponseDto>>(expiredVehiclesList);
        }

        public async Task<VehicleResponseDto> GetFullRecordByVehicleById(string id)
        {
            return await _vehicleRepo.GetVehicleById(id) ?? throw new Exception($"No data found for id {id}");
        }

        public async Task<VehicleResponseDto> UpdateVehicle(VehicleUpdateDto vehicle)
        {
            var existingVehicle = await _repo.GetByIdAsync(vehicle.VehicleId) ?? throw new Exception($"No data found for id {vehicle.VehicleId}");
            _mapper.Map(vehicle, existingVehicle);
            await _repo.Update(existingVehicle);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<VehicleResponseDto>(existingVehicle);
            return response;
        }

        public async Task<VehicleResponseDto> GetVehicleById(string id)
        {
            var response = _mapper.Map<VehicleResponseDto>(await _repo.GetByIdAsync(id));
            return response ?? throw new Exception($"No data found for id {id}");
        }

        public async Task<List<VehicleResponseDto>> GetAssignedVehicleList(VehicleStatus Status)
        {
            try
            {
                var response = await _vehicleRepo.GetAssignedVehicleList(Status);
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching {Status} vehicle list {ex.Message}");
            }
        }

        public async Task<List<VehicleResponseDto>> GetUnAssignedVehicleList(VehicleStatus Status)
        {
            try
            {
                var response = await _vehicleRepo.GetUnAssignedVehicleList(Status);
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching {Status} vehicle list {ex.Message}");
            }
        }


        public async Task<ApiResponse<VehicleResponseDto>> ChangeVehicleStatusAsync(string vehicleId, VehicleStatus status)
        {
            try
            {
                var response = await _vehicleRepo.ChangeStatusOfVehicle(vehicleId, status);
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching {status} vehicle list {ex.Message}");
            }
        }


        public async Task<ApiResponse<VehicleResponseDto>> GetDocumentOfVehicleById(string id)
        {
            try
            {
                var result = await _vehicleRepo.GetDocumentOfVehicleById(id);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching document of vehicle with id {id} {ex.Message}");
            }
        }

        public async Task<IEnumerable<VehicleResponseDto>> GetVehicleStatus(VehicleStatus vehicleStatus)
        {
            var result = await _vehicleRepo.GetVehicleStatusAsync(vehicleStatus);
            return result;
        }
    }
}
