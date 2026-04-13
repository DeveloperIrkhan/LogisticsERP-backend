using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.Services
{
    public class DriverService : IDriverService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Driver> _driverRepo;
        private readonly IGenericRepo<Vehicle> _vehicleRepo;

        public DriverService(IGenericRepo<Driver> genericDriverRepo, IGenericRepo<Vehicle> genericVehicleReop, IMapper mapper, AppDbContext appDbContext)
        {
            _driverRepo = genericDriverRepo;
            _vehicleRepo = genericVehicleReop;
            _context = appDbContext;
            _mapper = mapper;
        }
        public async Task<DriverResponseDto> AssignDriver(string vehicleId, string driverId)
        {
            var vehicle = await _vehicleRepo.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new Exception("vehicle not found!");
            }
            var driver = await _driverRepo.GetByIdAsync(driverId);
            if (driver == null) 
            {
                throw new NullReferenceException("driver not found!");
            }

            driver.DriverId = driverId;
            await _context.SaveChangesAsync();
            return _mapper.Map<DriverResponseDto>(driver);
        }

        public async Task<List<DriverResponseDto>> DriverListAssignedToSpecficVehicle(string vehicleId)
        {
            var vehicle = await _vehicleRepo.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new Exception("vehicle not found!");
            }
            var driversList = await _driverRepo.WhereAsync(d => d.VehicleId == vehicleId);
            return _mapper.Map<List<DriverResponseDto>>(driversList);
        }

        public async Task<DriverResponseDto> CreateDriver(DriverCreateDto driver)
        {
            if(driver == null)
            {
                throw new Exception("vehicle should not be empty!");
            }
            var newDriver = _mapper.Map<Driver>(driver);
            await _driverRepo.AddAsync(newDriver);
            await _context.SaveChangesAsync();
            return _mapper.Map<DriverResponseDto>(newDriver);
        }

        public Task DeleteVehicle(string id)
        {
            throw new NotImplementedException();
        }

     

        public Task<IEnumerable<DriverResponseDto>> GetAllDrivers()
        {
            throw new NotImplementedException();
        }

        public Task<DriverResponseDto> GetDriverById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DriverResponseDto> UpdateDriver(DriverUpdateDto vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
