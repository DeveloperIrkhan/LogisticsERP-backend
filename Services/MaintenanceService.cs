using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Maintenance;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace LogisticsERP.API.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepo _maintenanceRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IGenericRepo<MaintenanceRecord> _mainGenReop;
        private readonly IGenericRepo<Vehicle> _vehGenReop;


        //Helpers function

        private ApiResponse<T> Ok<T>(T data, string message) =>
        new()
        { Success = true, Message = message, Data = data };


        private ApiResponse<T> Fail<T>(string message) =>
        new()
        { Success = false, Message = message, Data = default };


        public MaintenanceService(
            IMaintenanceRepo maintenanceReop,
            IMapper mapper,
            AppDbContext dbContext,
            IGenericRepo<MaintenanceRecord> genericRepo,
            IGenericRepo<Vehicle> vehicleGenericRepo)
        {
            _maintenanceRepo = maintenanceReop;
            _mapper = mapper;
            _context = dbContext;
            _mainGenReop = genericRepo;
            _vehGenReop = vehicleGenericRepo;
        }


        public async Task<ApiResponse<MaintenanceResponseDto>> CreateAsync(MaintenanceCreateDto dto)
        {
            try
            {
                var vehicle = await _vehGenReop.GetByIdAsync(dto.VehicleId);
                if (vehicle == null)
                    return Fail<MaintenanceResponseDto>("Vehicle not found.");

                var records = _mapper.Map<MaintenanceRecord>(dto);
                await _mainGenReop.AddAsync(records);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<MaintenanceResponseDto>(records), "maintenance record added successfully");

            }
            catch (Exception ex)
            {
                return Fail<MaintenanceResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<MaintenanceResponseDto>> UpdateAsync(string MaintenanceRecordId, MaintenanceUpdateDto dto)
        {
            try
            {
                var haveRecord = await _mainGenReop.GetByIdAsync(MaintenanceRecordId);
                if (haveRecord == null) return Fail<MaintenanceResponseDto>("Maintenance record is not found.");

                if (dto.MaintenanceDate.HasValue) haveRecord.MaintenanceDate = dto.MaintenanceDate.Value;
                if (dto.CurrentKm.HasValue) haveRecord.CurrentKm = dto.CurrentKm.Value;
                if (dto.Cost.HasValue) haveRecord.Cost = dto.Cost.Value;
                if (dto.Description != null) haveRecord.Description = dto.Description;
                if (dto.MaintenanceType != null) haveRecord.MaintenanceType = dto.MaintenanceType;
                if (dto.WorkshopName != null) haveRecord.WorkshopName = dto.WorkshopName;
                if (dto.ChangedParts != null) haveRecord.ChangedParts = dto.ChangedParts;
                if (dto.InvoiceNumber != null) haveRecord.InvoiceNumber = dto.InvoiceNumber;
                if (dto.NextMaintenanceKm.HasValue) haveRecord.NextMaintenanceKm = dto.NextMaintenanceKm.Value;
                if (dto.NextMaintenanceDate.HasValue) haveRecord.NextMaintenanceDate = dto.NextMaintenanceDate.Value;

                await _mainGenReop.Update(haveRecord);
                await _context.SaveChangesAsync();

                return Ok(_mapper.Map<MaintenanceResponseDto>(haveRecord), "record successfully updated");

            }
            catch (Exception ex)
            {
                return Fail<MaintenanceResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<MaintenanceResponseDto>> GetByIdAsync(string MaintenanceRecordId)
        {
            try
            {
                var record = await _mainGenReop.GetByIdAsync(MaintenanceRecordId);
                if (record == null) return Fail<MaintenanceResponseDto>("maintenance record not found");

                return Ok(_mapper.Map<MaintenanceResponseDto>(record), "record fetched successfully");
            }
            catch (Exception ex)
            {
                return Fail<MaintenanceResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }

        }
        public async Task<ApiResponse<bool>> DeleteAsync(string MaintenanceRecordId)
        {
            try
            {
                var record = await _mainGenReop.GetByIdAsync(MaintenanceRecordId);
                if (record == null) return Ok(false, "maintenance record not found");

                await _mainGenReop.Delete(MaintenanceRecordId);
                await _context.SaveChangesAsync();
                return Ok(true, "Maintenance record deleted successfully.");

            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<MaintenanceResponseDto>>> GetAllAsync()
        {
            try
            {
                var records = await _mainGenReop.GetAllAsync();
                if (records == null) return Fail<List<MaintenanceResponseDto>>("maintenance record not found");
                var result = _mapper.Map<List<MaintenanceResponseDto>>(records);
                return Ok(result, $"{result.Count} record(s) found successfully");
            }
            catch (Exception ex)
            {
                return Fail<List<MaintenanceResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<MaintenanceResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                if (from > to) return Fail<List<MaintenanceResponseDto>>("'From' date cannot be greater than 'To' date.\"");


                var records = await _maintenanceRepo.GetByDateRangeAsync(from, to);
                var result = _mapper.Map<List<MaintenanceResponseDto>>(records);
                return Ok(result, $"{result.Count} record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<MaintenanceResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<MaintenanceResponseDto>>> GetByVehicleAsync(string VehicleId)
        {
            try
            {
                var records = await _maintenanceRepo.GetByVehicle(VehicleId);
                var result = _mapper.Map<List<MaintenanceResponseDto>>(records);
                return Ok(result, $"{result.Count} record(s) found for vehicle.");
            }
            catch (Exception ex)
            {
                return Fail<List<MaintenanceResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<MaintenanceCostReportDto>> GetTotalCostByVehicleAsync(string vehicleId)
        {
            try
            {
                var vehicle = await _vehGenReop.GetByIdAsync(vehicleId);
                if (vehicle == null) return Fail<MaintenanceCostReportDto>("vehicle not found.");

                var records = await _maintenanceRepo.GetByVehicle(vehicleId);

                var report = new MaintenanceCostReportDto
                {
                    TotalCost = records.Sum(x => x.Cost),
                    TotalRecords = records.Count
                };

                return Ok(report, "Total cost fetched successfully.");
            }

            catch (Exception ex)
            {
                return Fail<MaintenanceCostReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<MaintenanceCostReportDto>> GetMonthlyCostAsync(int year, int month)
        {
            try
            {
                var cost = await _maintenanceRepo.GetMonthlyCostAsync(year, month);
                var report = new MaintenanceCostReportDto
                {
                    Month = month,
                    Year = year,
                    TotalCost = cost
                };
                return Ok(report, $"Monthly cost for {year}/{month} fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<MaintenanceCostReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<MaintenanceCostReportDto>> GetYearlyCostAsync(int year)
        {
            try
            {
                var cost = await _maintenanceRepo.GetYearlyCostAsync(year);
                var report = new MaintenanceCostReportDto
                {
                    Year = year,
                    TotalCost = cost
                };
                return Ok(report, $"Monthly cost for {year} fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<MaintenanceCostReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<MaintenanceResponseDto>>> GetUpcomingMaintenanceAsync(int days)
        {
            try {
                if(days<=0) return Fail<List<MaintenanceResponseDto>>("Days must be a positive number.");

                var records = await _maintenanceRepo.GetUpcomingAsync(days);
                var result = _mapper.Map<List<MaintenanceResponseDto>>(records);

                return Ok(result, $"{result.Count} upcoming maintenance record(s) in the next {days} day(s).");
            }
            catch (Exception ex)
            {
                return Fail<List<MaintenanceResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
