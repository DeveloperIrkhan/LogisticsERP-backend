using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class DriverService : IDriverService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Driver> _driverGenRepo;
        private readonly IDriverRepo _driverRepo;
        private readonly IVehicleRepo _vehRepo;
        private readonly IGenericRepo<Vehicle> _vehicleGenRepo;

        public DriverService(IGenericRepo<Driver> genericDriverRepo, IGenericRepo<Vehicle> genericVehicleReop, IMapper mapper, AppDbContext appDbContext, IVehicleRepo vehicle, IDriverRepo driverRepo)
        {
            _driverGenRepo = genericDriverRepo;
            _driverRepo = driverRepo;
            _vehRepo = vehicle; 
            _vehicleGenRepo = genericVehicleReop;
            _context = appDbContext;
            _mapper = mapper;
        }
        public async Task<ApiResponse<DriverResponseDto>> AssignDriver(string driverId, string vehicleId)
        {
            var vehicle = await _vehicleGenRepo.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return new ApiResponse<DriverResponseDto>
                {
                    Success = false,
                    Message = "vehicle not found"
                };
            }
            var driver = await _driverGenRepo.GetByIdAsync(driverId);
            if (driver == null)
            {
                return new ApiResponse<DriverResponseDto>
                {
                    Success = false,
                    Message = "Driver not found"
                };
            }
            if (await _vehRepo.IsDriverAlreadyAssignedToAnotherVehicle(vehicleId, driverId))
            {
                return new ApiResponse<DriverResponseDto>
                {
                    Success = false,
                    Message = "Driver is already assigned to this vehicle"
                };
            }
            if(await _vehRepo.IsVehicleActive(vehicleId))
            {
                return new ApiResponse<DriverResponseDto>
                {
                    Success = false,
                    Message = "vehicle is inActive for now"
                };

            }
            //Driver is already assigned to this vehicle
           
            driver.VehicleId = vehicleId;
            await _context.SaveChangesAsync();
            return new ApiResponse<DriverResponseDto>
            {
                Success = true,
                Message = "Driver assigned successfully",
                Data = _mapper.Map<DriverResponseDto>(driver)
            };
        }

        public async Task<DriverResponseDto> UnassignDriver(string driverId)
        {
            var driver = await _driverGenRepo.GetByIdAsync(driverId) ?? throw new Exception("Driver not found!");
            if (string.IsNullOrEmpty(driver.VehicleId))
                throw new Exception("Driver is already unassigned!");
            driver.VehicleId = null;
            await _context.SaveChangesAsync();
            return _mapper.Map<DriverResponseDto>(driver);
        }

        public async Task<List<DriverResponseDto>> GetAssignedDriversListForSignleVehicle(string vehId) {
            var driverList = await _driverRepo.GerDriverListForSpecficVehicle(vehId);
            return driverList == null ? throw new Exception("no driver found!") : _mapper.Map<List<DriverResponseDto>>(driverList);   
        }

        public async Task<List<DriverResponseDto>> DriverListAssignedToSpecficVehicle(string vehicleId)
        {
            var vehicle = await _vehicleGenRepo.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new Exception("vehicle not found!");
            }
            var driversList = await _driverGenRepo.WhereAsync(d => d.VehicleId == vehicleId);
            return _mapper.Map<List<DriverResponseDto>>(driversList);
        }

        public async Task<DriverResponseDto> CreateDriver(DriverCreateDto driver)
        {
            if (driver == null)
            {
                throw new Exception("vehicle should not be empty!");
            }
            var newDriver = _mapper.Map<Driver>(driver);
            await _driverGenRepo.AddAsync(newDriver);
            await _context.SaveChangesAsync();
            return _mapper.Map<DriverResponseDto>(newDriver);
        }

        public Task DeleteVehicle(string id)
        {
            throw new NotImplementedException();
        }

        //return URL of cloudinar uploaded image save the url in database
        //public async Task<DriverResponseDto> ImageUploadingAsync(string FileUrlFileUrl)
        //{
        //    if (FileUrlFileUrl == null)
        //        throw new Exception("image url should not be empty!");
        //    await _


        //    await _docRepo.(newDocuments);
        //    await _dbContext.SaveChangesAsync();
        //}

        public async Task<IEnumerable<DriverResponseDto>> GetAllDrivers()
        {
            var result =  await _driverRepo.getAllDriverAsync();
            return _mapper.Map<IEnumerable<DriverResponseDto>>(result);
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
