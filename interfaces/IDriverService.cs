using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IDriverService
    {
        //crud
        Task<ApiResponse<DriverResponseDto>> CreateDriver(DriverCreateDto dto, string? PhotoUrl,string? LicenseUrl);
        Task<ApiResponse<DriverResponseDto>> UpdateDriver(DriverUpdateDto driver, string? PhotoUrl, string? LicenseUrl);
        Task<ApiResponse<bool>> DeleteDriver(string id);
        Task<ApiResponse<DriverResponseDto>> GetDriverById(string id);
        Task<ApiResponse<IEnumerable<DriverResponseDto>>> GetAllDrivers();


        //ASSIGNMENT 

        Task<ApiResponse<DriverResponseDto>> AssignDriver(string driverId, string vehicleId);
        Task<ApiResponse<DriverResponseDto>> UnassignDriver(string driverId);
        Task<ApiResponse<List<DriverResponseDto>>> DriverListAssignedToSpecficVehicle(string vehicleId);
        Task<ApiResponse<List<DriverResponseDto>>> GetAssignedDriversListForSignleVehicle(string vehicleId);

        //AVAILABILITY check
        Task<ApiResponse<List<DriverResponseDto>>> GetAvailableDriversAsync();
        Task<ApiResponse<List<DriverResponseDto>>> GetDriversByStatusAsync(DriverStatus status);
        Task<ApiResponse<bool>> ChangeDriverStatusAsync(string driverId, DriverStatus status);


        // ── DUTY TRACKING ─────────────────────────────────────
        Task<ApiResponse<DriverDutyStatsDto>> GetDriverDutyStatsAsync(string driverId);
        Task<ApiResponse<bool>> IsDriverAvailableAsync(string driverId);


        //alerts
        Task<ApiResponse<List<DriverResponseDto>>> GetExpiringLicensesAsync(int days);





    }
}
