using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class DriverService : ServiceBaseFunctions, IDriverService
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

        #region  crud
        //done
        public async Task<ApiResponse<DriverResponseDto>> CreateDriver(DriverCreateDto dto, string? PhotoUrl, string? LicenseUrl)
        {
            try
            {
                if (dto == null)
                    return Fail<DriverResponseDto>("Driver data is required.");

                if (await _driverRepo.IsCNICDuplicateAsync(dto.CNIC))
                    return Fail<DriverResponseDto>("A driver with this CNIC already exists.");

                if (await _driverRepo.IsLicenseDuplicateAsync(dto.LicenseNumber))
                    return Fail<DriverResponseDto>("A driver with this license number already exists.");

                var driver = _mapper.Map<Driver>(dto);
                driver.PhotoUrl = PhotoUrl;
                driver.LicenseUrl = LicenseUrl;

                await _driverGenRepo.AddAsync(driver);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DriverResponseDto>(driver), "Driver created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DriverResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteDriver(string id)
        {
            try
            {
                await _driverGenRepo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "driver record deleted successfully!");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<DriverResponseDto>>> GetAllDrivers()
        {
            try
            {
                var result = await _driverRepo.getAllDriverAsync();
                var drivers = _mapper.Map<IEnumerable<DriverResponseDto>>(result);
                return Ok(drivers, $"{drivers.Count()} drivers found successfully");
            }
            catch (Exception ex)
            {
                return Fail<IEnumerable<DriverResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<DriverResponseDto>> GetDriverById(string id)
        {
            try
            {

                var driver = await _driverGenRepo.GetByIdAsync(id);
                return Ok(_mapper.Map<DriverResponseDto>(driver), "driver found successfully");
            }
            catch (Exception ex)
            {
                return Fail<DriverResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DriverResponseDto>> UpdateDriver(DriverUpdateDto dto, string? PhotoUrl, string? LicenseUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.DriverId))
                    return Fail<DriverResponseDto>("DriverId is required.");

                var existingDriver = await _driverGenRepo.GetByIdAsync(dto.DriverId);
                if (existingDriver == null)
                    return Fail<DriverResponseDto>("driver not found");
                // Only update fields that are passed
                if (dto.FullName != null) existingDriver.FullName = dto.FullName;
                if (dto.CNIC != null) existingDriver.CNIC = dto.CNIC;
                if (dto.MobileNumber != null) existingDriver.MobileNumber = dto.MobileNumber;
                if (dto.Email != null) existingDriver.Email = dto.Email;
                if (dto.Address != null) existingDriver.Address = dto.Address;
                if (dto.LicenseNumber != null) existingDriver.LicenseNumber = dto.LicenseNumber;
                if (dto.LicenseExpiry.HasValue) existingDriver.LicenseExpiry = dto.LicenseExpiry.Value;
                if (dto.TypeOfLicence != null) existingDriver.typeOfLicence = dto.TypeOfLicence;
                if (dto.DateOfJoining.HasValue) existingDriver.DateOfJoining = dto.DateOfJoining.Value;
                if (dto.Status.HasValue) existingDriver.Status = dto.Status.Value;
                if (dto.Description != null) existingDriver.Description = dto.Description;
                if (dto.VehicleId != null) existingDriver.VehicleId = dto.VehicleId;

                // Only update photo/license if new files were uploaded
                if (PhotoUrl != null) existingDriver.PhotoUrl = PhotoUrl;
                if (LicenseUrl != null) existingDriver.LicenseUrl = LicenseUrl;



                await _driverGenRepo.Update(existingDriver);
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<DriverResponseDto>(existingDriver), "driver updated successfully");
            }
            catch (Exception ex)
            {
                return Fail<DriverResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        #endregion

        #region ASSIGNMENT
        
        public async Task<ApiResponse<DriverResponseDto>> AssignDriver(string driverId, string vehicleId)
        {
            try
            {
                var vehicle = await _vehicleGenRepo.GetByIdAsync(vehicleId);
                if (vehicle == null)
                    return Fail<DriverResponseDto>("vehicle not found.");
                var driver = await _driverGenRepo.GetByIdAsync(driverId);

                if (driver == null)
                    return Fail<DriverResponseDto>("Driver not found.");
                if (await _vehRepo.IsDriverAlreadyAssignedToAnotherVehicle(vehicleId, driverId))
                    return Fail<DriverResponseDto>("Driver is already assigned to this vehicle");

                if (!await _vehRepo.IsVehicleActive(vehicleId))
                    return Fail<DriverResponseDto>("vehicle is not active for now");

                driver.VehicleId = vehicleId;
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<DriverResponseDto>(driver), "driver assigned successfully!");
            }
            catch (Exception ex)
            {
                return Fail<DriverResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<DriverResponseDto>> UnassignDriver(string driverId)
        {
            try
            {
                var driver = await _driverGenRepo.GetByIdAsync(driverId);
                if (driver == null)
                    return Fail<DriverResponseDto>("Driver not found!");
                if (string.IsNullOrEmpty(driver.VehicleId))
                    return Fail<DriverResponseDto>("Driver is already unassigned!");
                driver.VehicleId = null;
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<DriverResponseDto>(driver), "driver un assigned successfully!");

            }
            catch (Exception ex)
            {
                return Fail<DriverResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<List<DriverResponseDto>>> GetAssignedDriversListForSignleVehicle(string vehicleId)
        {
            try
            {
                var driverList = await _driverRepo.GerDriversListForSpecficVehicle(vehicleId);
                if (driverList == null)
                    return Fail<List<DriverResponseDto>>("no driver found(s)");
                var listOfDrivers = _mapper.Map<List<DriverResponseDto>>(driverList);
                return Ok(listOfDrivers, $"{listOfDrivers.Count} drivers found successfully");
            }
            catch (Exception ex)
            {
                return Fail<List<DriverResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }

        }
        public async Task<ApiResponse<List<DriverResponseDto>>> DriverListAssignedToSpecficVehicle(string vehicleId)
        {
            try
            {

                var vehicle = await _vehicleGenRepo.GetByIdAsync(vehicleId);
                if (vehicle == null)
                    return Fail<List<DriverResponseDto>>("vehicle not found!");

                var driversList = await _driverGenRepo.WhereAsync(d => d.VehicleId == vehicleId);
                var driverList = _mapper.Map<List<DriverResponseDto>>(driversList);
                return Ok(driverList, $"{driverList.Count} drivers found successfully!");
            }
            catch (Exception ex)
            {
                return Fail<List<DriverResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }


        #endregion

        #region AVAILABILITY check
        public async Task<ApiResponse<bool>> ChangeDriverStatusAsync(string driverId, DriverStatus status)
        {
            try
            {
                var driver = await _driverGenRepo.GetByIdAsync(driverId);
                if (driver == null)
                    return Fail<bool>("Driver not found.");

                driver.Status = status;
                await _driverGenRepo.Update(driver);
                await _context.SaveChangesAsync();

                return Ok(true, $"Driver status changed to {status} successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<List<DriverResponseDto>>> GetAvailableDriversAsync()
        {
            try
            {
                var drivers = await _driverRepo.GetAvailableDriversAsync();
                var result = _mapper.Map<List<DriverResponseDto>>(drivers);
                return Ok(result, $"{result.Count} available driver(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<DriverResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DriverResponseDto>>> GetDriversByStatusAsync(DriverStatus status)
        {
            try
            {
                var drivers = await _driverRepo.GetDriversByStatusAsync(status);
                var result = _mapper.Map<List<DriverResponseDto>>(drivers);
                return Ok(result, $"{result.Count} driver(s) found with status {status}.");
            }
            catch (Exception ex)
            {
                return Fail<List<DriverResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

         public async Task<ApiResponse<DriverDutyStatsDto>> GetDriverDutyStatsAsync(string driverId)
        {
            try
            {
                var driver = await _driverGenRepo.GetByIdAsync(driverId);
                if (driver == null)
                    return Fail<DriverDutyStatsDto>("Driver not found.");

                var duties = await _context.DutyLogs
                    .Where(x => x.DriverId == driverId)
                    .ToListAsync();

                var isOnDuty = duties.Any(x => x.Status == DutyStatus.InProgress);

                var stats = new DriverDutyStatsDto
                {
                    DriverId = driverId,
                    DriverName = driver.FullName,
                    TotalDuties = duties.Count,
                    CompletedDuties = duties.Count(x => x.Status == DutyStatus.Completed),
                    MissedDuties = duties.Count(x => x.Status == DutyStatus.Cancelled),
                    CancelledDuties = duties.Count(x => x.Status == DutyStatus.Cancelled),
                    CurrentlyOnDuty = isOnDuty ? 1 : 0,
                    TotalKmDriven = duties.Where(x => x.TotalKm.HasValue).Sum(x => x.TotalKm!.Value),
                    TotalHours = duties.Where(x => x.TotalHours.HasValue).Sum(x => x.TotalHours!.Value),
                    LastDutyDate = duties.OrderByDescending(x => x.DateOut).FirstOrDefault()?.DateOut,
                    IsAvailable = !isOnDuty && driver.Status == DriverStatus.Active
                };

                return Ok(stats, "Driver duty stats fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DriverDutyStatsDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> IsDriverAvailableAsync(string driverId)
        {
            try
            {
                var driver = await _driverGenRepo.GetByIdAsync(driverId);
                if (driver == null)
                    return Fail<bool>("Driver not found.");

                var isOnDuty = await _context.DutyLogs
                    .AnyAsync(x => x.DriverId == driverId && x.Status == DutyStatus.InProgress);

                return Ok(!isOnDuty, isOnDuty ? "Driver is currently on duty." : "Driver is available.");
            }
            catch (Exception ex)
            { 
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        #endregion

        #region Alerts
        public async Task<ApiResponse<List<DriverResponseDto>>> GetExpiringLicensesAsync(int days)
        {
            try
            {
                var today = DateTime.UtcNow;
                var target = today.AddDays(days);

                var drivers = await _context.Drivers
                    .Where(x => x.LicenseExpiry >= today && x.LicenseExpiry <= target)
                    .OrderBy(x => x.LicenseExpiry)
                    .ToListAsync();

                var result = _mapper.Map<List<DriverResponseDto>>(drivers);
                return Ok(result, $"{result.Count} driver(s) with license expiring in {days} days.");
            }
            catch (Exception ex)
            {
                return Fail<List<DriverResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        #endregion
    }
}
