using LogisticsERP.API.DTOs.Fuel;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IFuelService
    {
        Task<ApiResponse<FuelResponseDto>> CreateAsync(FuelCreateDto fuelCreateDto);
        Task<ApiResponse<FuelResponseDto>> UpdateAsync(string id, FuelUpdateDto fuelCreateDto);
        Task<ApiResponse<FuelResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<FuelResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(string id);
        //_______ vehicle and driver base _________________________________________
        Task<ApiResponse<List<FuelResponseDto>>> GetByVehicleAsync(string vehicleId);
        Task<ApiResponse<List<FuelResponseDto>>> GetByDriverAsync(string driverId);
        //_____ Filtering __________________________________________________
        Task<ApiResponse<List<FuelResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to);

        // ___ Reporting ___________________________________________________________________
        Task<ApiResponse<FuelConsumptionReportDto>> GetConsumptionByVehicleAsync(string vehicleId);
        Task<ApiResponse<FuelCostReportDto>> GetMonthlyCostAsync(int year, int month);
        Task<ApiResponse<FuelCostReportDto>> GetYearlyCostAsync(int year);


    }
}
