using LogisticsERP.API.DTOs.Maintenance;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IMaintenanceService
    {
        // =========================
        // CRUD OPERATIONS
        // =========================

        Task<ApiResponse<MaintenanceResponseDto>> CreateAsync(MaintenanceCreateDto maintenanceRequestDto);
        Task<ApiResponse<MaintenanceResponseDto>> UpdateMaintenanceRecordAsync(string MaintenanceRecordId, MaintenanceUpdateDto maintenanceUpdateDto);
        Task<ApiResponse<MaintenanceResponseDto>> GetMaintenanceRecordById(string MaintenanceRecordId);
        Task<ApiResponse<MaintenanceResponseDto>> GetAllMaintenanceRecordAsync();
        Task<bool> DeleteMaintenanceRecordByIdAsync(string MaintenanceRecordId);

        // =========================
        // VEHICLE-BASED QUERIES
        // =========================
        Task<ApiResponse<List<MaintenanceResponseDto>>> GetByVehicleAsync(string VehicleId);

        // =========================
        // HISTORY & FILTERING
        // =========================
        Task<ApiResponse<List<MaintenanceResponseDto>>> GetVehicleHistoryAsync(string VehicleId);

        Task<ApiResponse<List<MaintenanceResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to);

        // =========================
        //  COST TRACKING
        // =========================

        Task<ApiResponse<decimal>> GetTotalCostByVehicleAsync(string vehicleId);
        Task<ApiResponse<decimal>> GetMonthlyCostAsync(int year, int month);
        Task<ApiResponse<decimal>> GetYearlyCostAsync(int year);

        // =========================
        // ALERTS / UPCOMING SERVICES
        // =========================
        Task<ApiResponse<List<MaintenanceResponseDto>>> GetUpcomingMaintenanceAsync(int days);
    }
}