using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IGenericRepo<Vehicle> _repo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public VehicleService(IGenericRepo<Vehicle> genericRepo, IMapper mapper, AppDbContext appDbContext)
        {
            _repo = genericRepo;
            _mapper = mapper;
            _context = appDbContext;
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

        public async Task<VehicleResponseDto> GetVehicleById(string id)
        {
            var response = _mapper.Map<VehicleResponseDto>(await _repo.GetByIdAsync(id));
            return response ?? throw new Exception($"No data found for id {id}");
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

        
    }
}
