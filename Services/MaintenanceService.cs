using LogisticsERP.API.DTOs.Maintenance;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepo _repo;

        public MaintenanceService(IMaintenanceRepo repo)
        {
            _repo = repo;
        }

        public Task<ApiResponse<MaintenanceResponseDto>> CreateAsync(MaintenanceCreateDto maintenanceRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMaintenanceRecordByIdAsync(string MaintenanceRecordId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<MaintenanceResponseDto>> GetAllMaintenanceRecordAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<MaintenanceResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<MaintenanceResponseDto>>> GetByVehicleAsync(string VehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<MaintenanceResponseDto>> GetMaintenanceRecordById(string MaintenanceRecordId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<decimal>> GetMonthlyCostAsync(int year, int month)
        {
            //            var cost = await _repo.WhereAsync(x =>
            //    x.MaintenanceDate.Year == year &&
            //    x.MaintenanceDate.Month == month
            //);
            //            return cost.Sum(x => x.Cost);
            throw new NotImplementedException();
        }

        public Task<ApiResponse<decimal>> GetTotalCostByVehicleAsync(string vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<MaintenanceResponseDto>>> GetUpcomingMaintenanceAsync(int days)
        {
            throw new NotImplementedException();
            //var today = DateTime.UtcNow;
            //var target = today.AddDays(days);

            //return await _repo.WhereAsync(x =>
            //    x.NextMaintenanceDate >= today &&
            //    x.NextMaintenanceDate <= target
            //);
        }

        public Task<ApiResponse<List<MaintenanceResponseDto>>> GetVehicleHistoryAsync(string VehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<decimal>> GetYearlyCostAsync(int year)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<MaintenanceResponseDto>> UpdateMaintenanceRecordAsync(string MaintenanceRecordId, MaintenanceUpdateDto maintenanceUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
