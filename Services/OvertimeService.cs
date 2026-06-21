using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Overtime;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using LogisticsERP.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class OvertimeService : ServiceBaseFunctions, IOvertimeService
    {
        private readonly IGenericRepo<OvertimeDuty> _genericRepo;
        private readonly IOvertimeRepo _overtimeRepo;
        private readonly IGenericRepo<Driver> _driverRepo;
        private readonly IGenericRepo<DutyLogs> _dutyRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public OvertimeService(
             IGenericRepo<OvertimeDuty> genericRepo,
            IOvertimeRepo overtimeRepo,
            IGenericRepo<Driver> driverRepo,
            IGenericRepo<DutyLogs> dutyRepo,
            IMapper mapper,
            AppDbContext context)
        {
            _genericRepo = genericRepo;
            _overtimeRepo = overtimeRepo;
            _driverRepo = driverRepo;
            _dutyRepo = dutyRepo;
            _mapper = mapper;
            _context = context;
        }
        public async Task<ApiResponse<OvertimeResponseDto>> ApproveAsync(string id, string approvedBy)
        {
            try
            {
                var overtime = await _genericRepo.GetByIdAsync(id);
                if (overtime == null)
                    return Fail<OvertimeResponseDto>("Overtime record not found.");

                if (overtime.Status == OvertimeStatus.Approved)
                    return Fail<OvertimeResponseDto>("Overtime is already approved.");

                overtime.Status = OvertimeStatus.Approved;
                overtime.ApprovedBy = approvedBy;

                await _genericRepo.Update(overtime);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<OvertimeResponseDto>(overtime), "Overtime approved successfully.");
            }
            catch (Exception ex)
            {
                return Fail<OvertimeResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<OvertimeResponseDto>> CreateAsync(OvertimeCreateDto dto)
        {
            try
            {
                var driver = await _driverRepo.GetByIdAsync(dto.DriverId);
                if (driver == null)
                    return Fail<OvertimeResponseDto>("Driver not found.");

                if (dto.DutyId != null)
                {
                    var duty = await _dutyRepo.GetByIdAsync(dto.DutyId);
                    if (duty == null)
                        return Fail<OvertimeResponseDto>("Duty not found.");
                }

                var overtime = _mapper.Map<OvertimeDuty>(dto);
                //calculating total hours rate amount 
                overtime.TotalAmount = dto.Hours * dto.RatePerHour;
                overtime.Status = OvertimeStatus.Pending;

                await _genericRepo.AddAsync(overtime);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<OvertimeResponseDto>(overtime), "Overtime record created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<OvertimeResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var overtime = await _genericRepo.GetByIdAsync(id);
                if (overtime == null)
                    return Fail<bool>("Overtime record not found.");

                await _genericRepo.Delete(id);
                await _context.SaveChangesAsync();
                return Ok(true, "Overtime record deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<OvertimeResponseDto>>> GetAllAsync()
        {
            try
            {
                var records = await _genericRepo.GetAllAsync();
                var result = _mapper.Map<List<OvertimeResponseDto>>(records);
                return Ok(result, $"{result.Count} overtime record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<OvertimeResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<OvertimeResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                if (from > to)
                    return Fail<List<OvertimeResponseDto>>("'From' date cannot be greater than 'To' date.");

                var records = await _overtimeRepo.GetByDateRangeAsync(from, to);
                var result = _mapper.Map<List<OvertimeResponseDto>>(records);
                return Ok(result, $"{result.Count} overtime record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<OvertimeResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<OvertimeResponseDto>>> GetByDriverAsync(string driverId)
        {
            try
            {
                var records = await _overtimeRepo.GetByDriverAsync(driverId);
                var result = _mapper.Map<List<OvertimeResponseDto>>(records);
                return Ok(result, $"{result.Count} overtime record(s) found for driver.");
            }
            catch (Exception ex)
            {
                return Fail<List<OvertimeResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<OvertimeResponseDto>>> GetByDutyAsync(string dutyId)
        {
            try
            {
                var records = await _overtimeRepo.GetByDutyAsync(dutyId);
                var result = _mapper.Map<List<OvertimeResponseDto>>(records);
                return Ok(result, $"{result.Count} overtime record(s) found for duty.");
            }
            catch (Exception ex)
            {
                return Fail<List<OvertimeResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<OvertimeResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var overtime = await _genericRepo.GetByIdAsync(id);
                if (overtime == null)
                    return Fail<OvertimeResponseDto>("Overtime record not found.");

                return Ok(_mapper.Map<OvertimeResponseDto>(overtime), "Overtime record fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<OvertimeResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<OvertimeResponseDto>>> GetByStatusAsync(OvertimeStatus status)
        {
            try
            {
                var records = await _overtimeRepo.GetByStatusAsync(status);
                var result = _mapper.Map<List<OvertimeResponseDto>>(records);
                return Ok(result, $"{result.Count} overtime record(s) found with status {status}.");
            }
            catch (Exception ex)
            {
                return Fail<List<OvertimeResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<OvertimeReportDto>> GetMonthlyReportByDriverAsync(string driverId, int year, int month)
        {
            try
            {
                var driver = await _driverRepo.GetByIdAsync(driverId);
                if (driver == null)
                    return Fail<OvertimeReportDto>("Driver not found.");

                var records = await _overtimeRepo.GetByMonthAsync(year, month);
                var driverRecords = records.Where(x => x.DriverId == driverId).ToList();

                var report = new OvertimeReportDto
                {
                    DriverId = driverId,
                    TotalHours = driverRecords.Sum(x => x.Hours),
                    TotalAmount = driverRecords
                    .Where(x => x.Status == OvertimeStatus.Approved)
                    .Sum(x => x.TotalAmount ?? 0),
                    TotalRecords = driverRecords.Count,
                    Month = month,
                    Year = year
                };

                return Ok(report, $"Monthly overtime report for {year}/{month} fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<OvertimeReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<OvertimeReportDto>> GetTotalByDriverAsync(string driverId)
        {
            try
            {
                var driver = await _driverRepo.GetByIdAsync(driverId);
                if (driver == null)
                    return Fail<OvertimeReportDto>("Driver not found.");

                var totalHours = await _overtimeRepo.GetTotalHoursByDriverAsync(driverId);
                var totalAmount = await _overtimeRepo.GetTotalAmountByDriverAsync(driverId);
                var records = await _overtimeRepo.GetByDriverAsync(driverId);

                var report = new OvertimeReportDto
                {
                    DriverId = driverId,
                    TotalHours = totalHours,
                    TotalAmount = totalAmount,
                    TotalRecords = records.Count
                };

                return Ok(report, "Total overtime report fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<OvertimeReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<OvertimeResponseDto>> RejectAsync(string id, string approvedBy)
        {
            try
            {
                var overtime = await _genericRepo.GetByIdAsync(id);
                if (overtime == null)
                    return Fail<OvertimeResponseDto>("Overtime record not found.");

                if (overtime.Status == OvertimeStatus.Rejected)
                    return Fail<OvertimeResponseDto>("Overtime is already rejected.");

                overtime.Status = OvertimeStatus.Rejected;
                overtime.ApprovedBy = approvedBy;

                await _genericRepo.Update(overtime);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<OvertimeResponseDto>(overtime), "Overtime rejected successfully.");
            }
            catch (Exception ex)
            {
                return Fail<OvertimeResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<OvertimeResponseDto>> UpdateAsync(string id, OvertimeUpdateDto dto)
        {
            try
            {
                var overtime = await _genericRepo.GetByIdAsync(id);
                if (overtime == null)
                    return Fail<OvertimeResponseDto>("Overtime record not found.");

                if (dto.Date.HasValue) overtime.Date = dto.Date.Value;
                if (dto.Hours.HasValue) overtime.Hours = dto.Hours.Value;
                if (dto.RatePerHour.HasValue) overtime.RatePerHour = dto.RatePerHour.Value;
                if (dto.Reason != null) overtime.Reason = dto.Reason;
                if (dto.Notes != null) overtime.Notes = dto.Notes;
                if (dto.Status.HasValue) overtime.Status = dto.Status.Value;

                // Recalculate TotalAmount if Hours or RatePerHour changed
                if (dto.Hours.HasValue || dto.RatePerHour.HasValue)
                    overtime.TotalAmount = overtime.Hours * overtime.RatePerHour;

                await _genericRepo.Update(overtime);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<OvertimeResponseDto>(overtime), "Overtime record updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<OvertimeResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
