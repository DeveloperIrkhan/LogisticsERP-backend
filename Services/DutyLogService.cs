using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.DutyLogs;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using LogisticsERP.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class DutyLogService : ServiceBaseFunctions, IDutyLogService
    {
        private readonly IGenericRepo<DutyLogs> _genericRepo;
        private readonly IDutyRepo _dutyRepo;
        private readonly IGenericRepo<Vehicle> _vehicleRepo;
        private readonly IGenericRepo<Driver> _driverRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public DutyLogService(
            IGenericRepo<DutyLogs> genericRepo,
            IDutyRepo dutyRepo,
            IGenericRepo<Vehicle> vehicleRepo,
            IGenericRepo<Driver> driverRepo,
            IMapper mapper,
            AppDbContext context)
        {
            _genericRepo = genericRepo;
            _dutyRepo = dutyRepo;
            _vehicleRepo = vehicleRepo;
            _driverRepo = driverRepo;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ApiResponse<DutyResponseDto>> CreateAsync(DutyCreateDto dto)
        {
            try
            {
                var vehicle = await _vehicleRepo.GetByIdAsync(dto.VehicleId);
                if (vehicle == null) 
                    return Fail<DutyResponseDto>("vehicle not found");
                var driver = await _driverRepo.GetByIdAsync(dto.DriverId);
                if (driver == null) 
                    return Fail<DutyResponseDto>("driver not found");

                var duty = _mapper.Map<DutyLogs>(dto);
                duty.Status = DutyStatus.Pending;

                await _genericRepo.AddAsync(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty Create Successfully");
            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> UpdateAsync(string Id, DutyUpdateDto dto)
        {
            try 
            {
                var duty = await _genericRepo.GetByIdAsync(Id);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                //updating the desire fields
                if (dto.FromLocation != null) duty.FromLocation = dto.FromLocation;
                if (dto.ToLocation != null) duty.ToLocation = dto.ToLocation;
                if (dto.Purpose != null) duty.Purpose = dto.Purpose;
                if (dto.OfficerName != null) duty.OfficerName = dto.OfficerName;
                if (dto.Donor != null) duty.Donor = dto.Donor;
                if (dto.Remarks != null) duty.Remarks = dto.Remarks;
                if (dto.DutyType.HasValue) duty.DutyType = dto.DutyType.Value;
                if (dto.Status.HasValue) duty.Status = dto.Status.Value;
                if (dto.DateIn.HasValue) duty.DateIn = dto.DateIn.Value;
                if (dto.KillometerOut.HasValue) duty.KillometerOut = dto.KillometerOut.Value;
                if (dto.KillometerIn.HasValue) duty.KillometerIn = dto.KillometerIn.Value;
                if (dto.TotalKm.HasValue) duty.TotalKm = dto.TotalKm.Value;
                if (dto.TotalHours.HasValue) duty.TotalHours = dto.TotalHours.Value;

                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty updated successfully.");

            }
            catch (Exception exp)
            {
                return Fail<DutyResponseDto>(exp.InnerException?.Message ?? exp.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(id);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetAllAsync()
        {
            try 
            {
                var duties = await _genericRepo.GetAllAsync();
                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} duty record(s) found.");
            }
            catch(Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(id);
                if (duty == null)
                    return Fail<bool>("Duty not found.");

                await _genericRepo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "Duty deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }
               
        public async Task<ApiResponse<List<DutyResponseDto>>> GetByDriverAsync(string driverId)
        {
            try
            {
                var existingDriver = await _genericRepo.GetByIdAsync(driverId);
                if (existingDriver == null) return Fail<List<DutyResponseDto>>("driver not found");
                var drivers = _mapper.Map<List<DutyResponseDto>>(existingDriver);
                return Ok(drivers, $"{drivers.Count} duty record(s) found for driver.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetByStatusAsync(DutyStatus status)
        {
            try
            {
                var duties = await _dutyRepo.GetByStatusAsync(status);
                if (duties == null) return Fail<List<DutyResponseDto>>("no duty found for this status!");

                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} duty record(s) found with status {status}.");
            }
             catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetByVehicleAsync(string vehicleId)
        {
            try{
                var duties= await _dutyRepo.GetByVehicleAsync(vehicleId);
                if (duties == null) return Fail<List<DutyResponseDto>>("duties not found");

                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} duty record(s) found for vehicle.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>> (ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> StartDutyAsync(string dutyId)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(dutyId);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                if(duty.Status == DutyStatus.InProgress)
                    return Fail<DutyResponseDto>("Duty already in progress.");
                if(duty.Status == DutyStatus.Completed)
                    return Fail<DutyResponseDto>("Duty already in Completed.");

                duty.Status = DutyStatus.InProgress;
                duty.DateOut = DateTime.UtcNow;

                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty started successfully.");
            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<DutyResponseDto>> EndDutyAsync(string dutyId, EndDutyDto dto) 
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(dutyId);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                if (duty.Status == DutyStatus.Completed)
                    return Fail<DutyResponseDto>("Duty already in Completed.");
                if (duty.Status == DutyStatus.Pending)
                    return Fail<DutyResponseDto>("Duty not started yet.");

                duty.Status = DutyStatus.Completed;
                duty.DateIn = dto.DateIn;
                duty.KillometerIn = dto.KillometerIn;

                if (duty.KillometerOut.HasValue)
                    duty.TotalKm = dto.KillometerIn - duty.KillometerOut.Value;

                duty.TotalHours = (decimal)(dto.DateIn - duty.DateOut).TotalHours;

                if (dto.Remarks != null)
                    duty.Remarks = dto.Remarks;


                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty started successfully.");
            
            }
             catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetActiveDutiesAsync()
        {
            try
            {
                var duties = await _dutyRepo.GetActiveDutiesAsync();
                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} active duty record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> ApproveDutyAsync(string dutyId, string approvedBy)
        {
            try
            {
                var duty = await _genericRepo.GetByIdAsync(dutyId);
                if (duty == null)
                    return Fail<DutyResponseDto>("Duty not found.");

                if (duty.Status != DutyStatus.Completed)
                    return Fail<DutyResponseDto>("Only completed duties can be approved.");
                duty.ApprovedBy = approvedBy;
                duty.Status = DutyStatus.Approved;

                await _genericRepo.Update(duty);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty approved successfully.");

            }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<DutyResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                if (from > to)
                    return Fail<List<DutyResponseDto>>("'From' date cannot be greater than 'To' date.");

                var duties = await _dutyRepo.GetByDateRangeAsync(from, to);
                var result = _mapper.Map<List<DutyResponseDto>>(duties);
                return Ok(result, $"{result.Count} duty record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<DutyResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<DutyResponseDto>> CancelDutyAsync(string dutyId, string reason)
        {
            try {
                    var duty = await _genericRepo.GetByIdAsync(dutyId);
                    if (duty == null)
                        return Fail<DutyResponseDto>("Duty not found.");

                    if (duty.Status == DutyStatus.Completed)
                        return Fail<DutyResponseDto>("Completed duty cannot be cancelled.");

                    if (duty.Status == DutyStatus.Cancelled)
                        return Fail<DutyResponseDto>("Duty is already cancelled.");

                    duty.Status = DutyStatus.Cancelled;
                    duty.CancellationReason = reason;
                    duty.CancelledAt = DateTime.UtcNow;

                    await _genericRepo.Update(duty);
                    await _context.SaveChangesAsync();

                    return Ok(_mapper.Map<DutyResponseDto>(duty), "Duty cancelled successfully.");
                }
            catch (Exception ex)
            {
                return Fail<DutyResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }

        }
    }
}
 