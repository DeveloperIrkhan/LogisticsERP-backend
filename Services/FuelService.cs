using AutoMapper;
using CloudinaryDotNet.Actions;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Fuel;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using LogisticsERP.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class FuelService : IFuelService
    {
        private readonly IGenericRepo<FuelRecord> _genericRepo;
        private readonly IFuelRepo _fuelRepo;
        private readonly IGenericRepo<Vehicle> _genVehRepo;
        private readonly IGenericRepo<Driver> _genDrvRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _appContext;

        //helpers
        private ApiResponse<T> Ok<T>(T data, string message) =>
        new()
        { Success = true, Message = message, Data = data };

        private ApiResponse<T> Fail<T>(string message) =>
        new()
        { Success = false, Message = message, Data = default };

        public FuelService(
            IGenericRepo<FuelRecord> genericRepo,
            IFuelRepo fuelRepo,
            IGenericRepo<Vehicle> genVehRepo,
            IGenericRepo<Driver> genDrvRepo,
            IMapper mapper,
            AppDbContext appContext
            )
        {
            _genericRepo = genericRepo;
            _fuelRepo = fuelRepo;
            _genVehRepo = genVehRepo;
            _genDrvRepo = genDrvRepo;
            _mapper = mapper;
            _appContext = appContext;
        }



        public async Task<ApiResponse<FuelResponseDto>> CreateAsync(FuelCreateDto fuelCreateDto)
        {
            try
            {
                var vehicle = await _genVehRepo.GetByIdAsync(fuelCreateDto.VehicleId);
                if (vehicle == null)
                    return Fail<FuelResponseDto>("Vehicle not found.");

                var driver = await _genDrvRepo.GetByIdAsync(fuelCreateDto.DriverId);
                if (driver == null)
                    return Fail<FuelResponseDto>("Driver not found.");

                var record = _mapper.Map<FuelRecord>(fuelCreateDto);
                record.Vehicle = vehicle;  
                record.Driver = driver;
                await _genericRepo.AddAsync(record);
                await _appContext.SaveChangesAsync();

                return Ok(_mapper.Map<FuelResponseDto>(record), "Fuel record created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<FuelResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<FuelResponseDto>> UpdateAsync(string id, FuelUpdateDto fuelCreateDto)
        {
            try
            {
                var record = await _genericRepo.GetByIdAsync(id);
                if (record == null)
                    return Fail<FuelResponseDto>("Fuel record not found.");


                if (fuelCreateDto.DriverId != null) record.DriverId = fuelCreateDto.DriverId;
                if (fuelCreateDto.FuelingDate.HasValue) record.FuelingDate = fuelCreateDto.FuelingDate.Value;
                if (fuelCreateDto.OdoMeterReading.HasValue) record.OdoMeterReading = fuelCreateDto.OdoMeterReading.Value;
                if (fuelCreateDto.Liters.HasValue) record.Liters = fuelCreateDto.Liters.Value;
                if (fuelCreateDto.CostPerLiter.HasValue) record.CostPerLiter = fuelCreateDto.CostPerLiter.Value;
                if (fuelCreateDto.TotalCost.HasValue) record.TotalCost = fuelCreateDto.TotalCost.Value;
                if (fuelCreateDto.IsFullTank.HasValue) record.IsFullTank = fuelCreateDto.IsFullTank.Value;
                if (fuelCreateDto.Mileage.HasValue) record.Mileage = fuelCreateDto.Mileage.Value;
                if (fuelCreateDto.StationName != null) record.StationName = fuelCreateDto.StationName;
                if (fuelCreateDto.StationLocation != null) record.StationLocation = fuelCreateDto.StationLocation;
                if (fuelCreateDto.Province != null) record.Province = fuelCreateDto.Province;
                if (fuelCreateDto.ReceiptNumber != null) record.ReceiptNumber = fuelCreateDto.ReceiptNumber;
                if (fuelCreateDto.FuelType != null) record.FuelType = fuelCreateDto.FuelType;
                if (fuelCreateDto.PaymentMethod != null) record.PaymentMethod = fuelCreateDto.PaymentMethod;
                if (fuelCreateDto.Donor != null) record.Donor = fuelCreateDto.Donor;
                if (fuelCreateDto.Notes != null) record.Notes = fuelCreateDto.Notes;

                await _genericRepo.Update(record);
                await _appContext.SaveChangesAsync();

                return Ok(_mapper.Map<FuelResponseDto>(record), "Fuel record updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<FuelResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<FuelResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var record = await _fuelRepo.GetByIdAsync(id);
                if (record == null)
                    return Fail<FuelResponseDto>("Fuel record not found.");

                return Ok(_mapper.Map<FuelResponseDto>(record), "Fuel record fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<FuelResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                await _genericRepo.Delete(id);
                await _appContext.SaveChangesAsync();
                return Ok(true, "fuel record deleted successfully!");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ApiResponse<List<FuelResponseDto>>> GetAllAsync()
        {
            try
            {
                var records = await _fuelRepo.GetAll();
                var result = _mapper.Map<List<FuelResponseDto>>(records);
                return Ok(result, $"{result.Count} fuel record(s) found");
            }
            catch (Exception ex)
            {
                return Fail<List<FuelResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<FuelResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                if (from > to)
                    return Fail<List<FuelResponseDto>>("'From' date cannot be greater than 'To' date.");

                var records = await _fuelRepo.GetByDateRangeAsync(from, to);
                var result = _mapper.Map<List<FuelResponseDto>>(records);
                return Ok(result, $"{result.Count} fuel record(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<FuelResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<FuelResponseDto>>> GetByDriverAsync(string driverId)
        {
            try
            {
                var records = await _fuelRepo.GetByDriverAsync(driverId);
                var result = _mapper.Map<List<FuelResponseDto>>(records);
                return Ok(result, $"{result.Count} fuel record(s) found for driver.");
            }
            catch (Exception ex)
            {
                return Fail<List<FuelResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<List<FuelResponseDto>>> GetByVehicleAsync(string vehicleId)
        {
            try
            {
                var records = await _fuelRepo.GetByVehicleAsync(vehicleId);
                var result = _mapper.Map<List<FuelResponseDto>>(records);
                return Ok(result, $"{result.Count} fuel record found(s) for vehicle");
            }
            catch (Exception ex)
            {
                return Fail<List<FuelResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<FuelConsumptionReportDto>> GetConsumptionByVehicleAsync(string vehicleId)
        {
            try
            {
                var vehicle = await _genVehRepo.GetByIdAsync(vehicleId);
                if (vehicle == null) return Fail<FuelConsumptionReportDto>("no vehicle found.");

                var record = await _fuelRepo.GetByVehicleAsync(vehicleId);
                var getLastRecord = await _fuelRepo.GetLastRecordByVehicleAsync(vehicleId);

                var report = new FuelConsumptionReportDto
                {
                    VehicleId = vehicleId,
                    TotalLiters = record.Sum(x => x.Liters),
                    TotalCost = record.Sum(x => x.TotalCost),
                    TotalRecords = record.Count,
                    AverageMileage = record.Any(x => x.Mileage.HasValue) ? (int?)record.Where(x => x.Mileage.HasValue).Average(x => x.Mileage!.Value) : null,
                    LastFuelingDate = getLastRecord?.FuelingDate,
                };
                return Ok(report, "Consumption report fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<FuelConsumptionReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<FuelCostReportDto>> GetMonthlyCostAsync(int year, int month)
        {
            try
            {
                var cost = await _fuelRepo.GetMonthlyCostAsync(year, month);
                var liters = await _fuelRepo.GetMonthlyLitersAsync(year, month);


                var report = new FuelCostReportDto
                {
                    Month = month,
                    Year = year,
                    TotalCost = cost,
                    TotalLiters = liters

                };
                return Ok(report, $"Monthly fuel report for {year}/{month} fetched successfully.");

            }
            catch (Exception ex)
            {
                return Fail<FuelCostReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<FuelCostReportDto>> GetYearlyCostAsync(int year)
        {
            try
            {
                var cost = await _fuelRepo.GetYearlyCostAsync(year);
                var liters = await _fuelRepo.GetYearlyLitersAsync(year);

                var report = new FuelCostReportDto
                {
                    Year = year,
                    TotalCost = cost,
                    TotalLiters = liters
                };
                return Ok(report, $"Yearly fuel report for {year} fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<FuelCostReportDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
