using LogisticsERP.API.DTOs.Maintenance;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IMaintenanceService
    {
        // =========================
        // CRUD OPERATIONS
        // =========================

        Task<ApiResponse<MaintenanceResponseDto>> CreateAsync(MaintenanceCreateDto dto);
        Task<ApiResponse<MaintenanceResponseDto>> UpdateAsync(string MaintenanceRecordId, MaintenanceUpdateDto dto);
        Task<ApiResponse<MaintenanceResponseDto>> GetByIdAsync(string MaintenanceRecordId);
        Task<ApiResponse<List<MaintenanceResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(string MaintenanceRecordId);

        // =========================
        // VEHICLE-BASED QUERIES
        // =========================
        Task<ApiResponse<List<MaintenanceResponseDto>>> GetByVehicleAsync(string VehicleId);

        // =========================
        // HISTORY & FILTERING
        // =========================
        Task<ApiResponse<List<MaintenanceResponseDto>>> GetByDateRangeAsync(DateTime from, DateTime to);

        // =========================
        //  COST TRACKING
        // =========================

        Task<ApiResponse<MaintenanceCostReportDto>> GetTotalCostByVehicleAsync(string vehicleId);
        Task<ApiResponse<MaintenanceCostReportDto>> GetMonthlyCostAsync(int year, int month);
        Task<ApiResponse<MaintenanceCostReportDto>> GetYearlyCostAsync(int year);

        // =========================
        // ALERTS / UPCOMING SERVICES
        // =========================
        Task<ApiResponse<List<MaintenanceResponseDto>>> GetUpcomingMaintenanceAsync(int days);
    }
}