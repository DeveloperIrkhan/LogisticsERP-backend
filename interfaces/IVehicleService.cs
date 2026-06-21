using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IVehicleService
    {
        Task<ApiResponse<VehicleResponseDto>> CreateVehicle(VehicleCreateDto vehicle);
        Task<ApiResponse<VehicleResponseDto>> UpdateVehicle(VehicleUpdateDto vehicle);
        Task<ApiResponse<bool>> DeleteVehicle(string id);
        Task<ApiResponse<VehicleResponseDto>> GetVehicleById(string id);
        Task<ApiResponse<List<VehicleResponseDto>>> GetAssignedVehicleList(VehicleStatus Status);
        Task<ApiResponse<List<VehicleResponseDto>>> GetUnAssignedVehicleList(VehicleStatus Status);
        Task<ApiResponse<IEnumerable<VehicleResponseDto>>> GetAllVehicles();
        Task<ApiResponse<List<VehicleResponseDto>>> GetRegisterationExpiryVehicles(int days);
        Task<ApiResponse<List<VehicleResponseDto>>> GetFittnessExpiryVehicles(int days);
        Task<ApiResponse<List<VehicleResponseDto>>> GetInsuranceExpiryVehicles(int days);
        Task<ApiResponse<VehicleResponseDto>> GetFullRecordByVehicleById(string id);
        Task<ApiResponse<VehicleResponseDto>> GetDocumentsOfVehicleById(string id);
        Task<ApiResponse<List<VehicleResponseDto>>> GetVehicles(VehicleFilterDto vehicleFilter);
        Task<ApiResponse<VehicleResponseDto>> ChangeVehicleStatusAsync(string vehicleId, VehicleStatus status);
        Task<ApiResponse<IEnumerable<VehicleResponseDto>>> GetVehicleStatus(VehicleStatus vehicleStatus);
    }
}
