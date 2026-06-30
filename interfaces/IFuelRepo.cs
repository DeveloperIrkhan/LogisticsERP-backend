using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IFuelRepo
    {
        Task<List<FuelRecord>> GetAll();
        Task<FuelRecord> GetByIdAsync(string fuelId);
        Task<List<FuelRecord>> GetByVehicleAsync(string vehicleId);
        Task<List<FuelRecord>> GetByDriverAsync(string driverId);
        Task<List<FuelRecord>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<decimal> GetTotalCostByVehicleAsync(string vehicleId);
        Task<decimal> GetTotalLitersByVehicleAsync(string vehicleId);
        Task<decimal> GetMonthlyCostAsync(int year, int month);
        Task<decimal> GetMonthlyLitersAsync(int year, int month);
        Task<decimal> GetYearlyCostAsync(int year);
        Task<decimal> GetYearlyLitersAsync(int year);
        Task<FuelRecord?> GetLastRecordByVehicleAsync(string vehicleId);
    }
}
