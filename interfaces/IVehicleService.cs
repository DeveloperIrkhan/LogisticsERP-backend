using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IVehicleService
    {
        Task<VehicleResponseDto> CreateVehicle(VehicleCreateDto vehicle);
        Task<VehicleResponseDto> UpdateVehicle(VehicleUpdateDto vehicle);
        Task DeleteVehicle(string id);
        Task<VehicleResponseDto> GetVehicleById(string id);
        Task<List<VehicleResponseDto>> GetAssignedVehicleList(VehicleStatus Status);
        Task<List<VehicleResponseDto>> GetUnAssignedVehicleList(VehicleStatus Status);
        Task<IEnumerable<VehicleResponseDto>> GetAllVehicles();
        Task<List<VehicleResponseDto>> GetExpiringVehicles(int days);
        Task<VehicleResponseDto> GetFullRecordByVehicleById(string id);
        Task<ApiResponse<VehicleResponseDto>> GetDocumentOfVehicleById(string id);
        Task<List<VehicleResponseDto>> GetVehicles(VehicleFilterDto vehicleFilter);

        Task<ApiResponse<VehicleResponseDto>> ChangeVehicleStatusAsync(string vehicleId, VehicleStatus status);


        Task<IEnumerable<VehicleResponseDto>> GetVehicleStatus(VehicleStatus vehicleStatus);
    }
}
