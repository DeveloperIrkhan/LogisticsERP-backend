using AutoMapper;
using CloudinaryDotNet.Actions;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.Services
{
    public class VehicleService : ServiceBaseFunctions, IVehicleService
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

        public async Task<ApiResponse<List<VehicleResponseDto>>> GetVehicles(VehicleFilterDto vehicleFilter)
        {
            try
            {
                var vehicles = await _vehicleRepo.GetVehicles(vehicleFilter);
                var vehiclesList = _mapper.Map<List<VehicleResponseDto>>(vehicles);
                return Ok(vehiclesList,$"{vehiclesList.Count} vehicle found successfully!");
            }
            catch(Exception ex)
            {
                return Fail<List<VehicleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<VehicleResponseDto>> CreateVehicle(VehicleCreateDto vehicle)
        {
            try
            {
                if (vehicle == null)
                    return Fail<VehicleResponseDto>("vehicle should not be empty");

                var newVehicle = _mapper.Map<Vehicle>(vehicle);
                await _repo.AddAsync(newVehicle);
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<VehicleResponseDto>(newVehicle), "vehicle created successfully");

            }
            catch (Exception ex)
            {
                return Fail<VehicleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteVehicle(string id)
        {
            try
            {
                var founded = await _vehicleRepo.GetVehicleById(id);
                if (founded == null)
                    return Fail<bool>("vehicle not found");

                await _repo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "vehicle deleted successfully");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<VehicleResponseDto>>> GetAllVehicles()
        {
            try
            {

            var result = await _repo.GetAllAsync();
            var listOfVehicles = _mapper.Map<IEnumerable<VehicleResponseDto>>(result);
            return Ok(listOfVehicles, $"{listOfVehicles.Count()} vehicle found(s)!");
            }
            catch (Exception ex)
            {
                return Fail<IEnumerable<VehicleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<VehicleResponseDto>>> GetRegisterationExpiryVehicles(int days)
        {
            try {

                var today = DateTime.UtcNow;
                var nextDates = today.AddDays(days);
                var expiredVehiclesList = (await _repo.WhereAsync(
                    v => v.RegistrationExpiry >= today && v.RegistrationExpiry <= nextDates))
                    .OrderByDescending(v => v.RegistrationDate).ToList();
                var list = _mapper.Map<List<VehicleResponseDto>>(expiredVehiclesList);
                return Ok(list, $"{list.Count} vehicles are found");
            }
            catch (Exception ex)
            {
                return Fail<List<VehicleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }

        }
        public async Task<ApiResponse<List<VehicleResponseDto>>> GetFittnessExpiryVehicles(int days)
        {
            try {
                var today = DateTime.UtcNow;
                var nextDates = today.AddDays(days);
                var expiredVehiclesList = (await _repo.WhereAsync(v =>
                v.FitnessExpiry >= today && v.FitnessExpiry <= nextDates))
                    .OrderByDescending(v => v.FitnessExpiry).ToList();
                var list = _mapper.Map<List<VehicleResponseDto>>(expiredVehiclesList);
                return Ok(list, $"{list.Count} vehicles are found");
            }
            catch (Exception ex)
            {
                return Fail<List<VehicleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<List<VehicleResponseDto>>> GetInsuranceExpiryVehicles(int days)
        {
            try {
                var today = DateTime.UtcNow;
                var nextDates = today.AddDays(days);
                var expiredVehiclesList = (await _repo.WhereAsync(
                    v => v.InsuranceExpiry >= today && v.InsuranceExpiry <= nextDates))
                    .OrderByDescending(v => v.InsuranceExpiry).ToList();
                var list = _mapper.Map<List<VehicleResponseDto>>(expiredVehiclesList);
                return Ok(list, $"{list.Count} vehicles are found");
            }
            catch (Exception ex)
            {
                return Fail<List<VehicleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<VehicleResponseDto>> GetFullRecordByVehicleById(string id)
        {
            try {
                var found = await _vehicleRepo.GetVehicleById(id);
                if (found == null)
                    return Fail<VehicleResponseDto>("vehicle not found");
                
                return Ok(_mapper.Map<VehicleResponseDto>(found), "vehicle found");
            }
            catch (Exception ex)
            {
                return Fail<VehicleResponseDto> (ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<VehicleResponseDto>> UpdateVehicle(VehicleUpdateDto vehicle)
        {
            try
            {
                var existingVehicle = await _repo.GetByIdAsync(vehicle.VehicleId);
                if (existingVehicle == null)
                    return Fail<VehicleResponseDto>($"No data found for id {vehicle.VehicleId}");

                _mapper.Map(vehicle, existingVehicle);
                await _repo.Update(existingVehicle);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<VehicleResponseDto>(existingVehicle), "Vehicle updated successfully.");

            }
            catch (Exception ex)
            {
                return Fail<VehicleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<VehicleResponseDto>> GetVehicleById(string id)
        {
            try
            {
                var vehicle = await _repo.GetByIdAsync(id);
                if (vehicle == null)
                    return Fail<VehicleResponseDto>($"No data found for id {id}.");

                return Ok(_mapper.Map<VehicleResponseDto>(vehicle), "Vehicle fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<VehicleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<VehicleResponseDto>>> GetAssignedVehicleList(VehicleStatus status)
        {
            try
            {
                var result = await _vehicleRepo.GetAssignedVehicleList(status);
                if (result == null)
                    return Fail<List<VehicleResponseDto>>($"no vehicle(s) found for {status}");
                var AssignedVehicleList = _mapper.Map<List<VehicleResponseDto>>(result);
                return Ok(AssignedVehicleList, $"{AssignedVehicleList.Count} assigned vehicle(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<VehicleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<VehicleResponseDto>>> GetUnAssignedVehicleList(VehicleStatus status)
        {
            try
            {
                var result = await _vehicleRepo.GetUnAssignedVehicleList(status);
                if (result == null)
                    return Fail<List<VehicleResponseDto>>($"no vehicle(s) found for {status}");
                var vehicle = _mapper.Map<List<VehicleResponseDto>>(result);
                return Ok(vehicle, $"{vehicle.Count} unassigned vehicle(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<VehicleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        public async Task<ApiResponse<VehicleResponseDto>> ChangeVehicleStatusAsync(string vehicleId, VehicleStatus status)
        {
            try
            {
                var result = await _vehicleRepo.ChangeStatusOfVehicle(vehicleId, status);
                if (result == null)
                    return Fail<VehicleResponseDto>("failed to changed the status");
                await _context.SaveChangesAsync();
                var vehicle = _mapper.Map<VehicleResponseDto>(result);
                return Ok(vehicle, "vehicle Status changed.");

            }
            catch (Exception ex)
            {
                return Fail<VehicleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }



        public async Task<ApiResponse<VehicleResponseDto>> GetDocumentsOfVehicleById(string id)
        {
            try
            {
                var result = await _vehicleRepo.GetDocumentOfVehicleById(id);
                if (result == null)
                    return Fail<VehicleResponseDto>($"No documents found for vehicle {id}.");
                var vehicle = _mapper.Map<VehicleResponseDto>(result);
                return Ok(vehicle, "vehicle's documents found successfully!");
            }
            catch (Exception ex)
            {
                return Fail<VehicleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<VehicleResponseDto>>> GetVehicleStatus(VehicleStatus vehicleStatus)
        {
            try
            {
                var result = await _vehicleRepo.GetVehicleStatusAsync(vehicleStatus);
                if (result == null)
                    return Fail<IEnumerable<VehicleResponseDto>>($"vehicles found(s) for {vehicleStatus}");
                var vehicles = _mapper.Map<IEnumerable<VehicleResponseDto>> (result);
                return Ok(vehicles, "Vehicle status list fetched successfully");
            }
            catch (Exception ex)
            {
                return Fail<IEnumerable<VehicleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        
    }
}
