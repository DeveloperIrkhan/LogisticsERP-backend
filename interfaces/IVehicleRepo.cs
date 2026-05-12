using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;
using System.Collections.Specialized;
using System.Net.NetworkInformation;

namespace LogisticsERP.API.interfaces
{
    public interface IVehicleRepo
    {
        Task<VehicleResponseDto> GetVehicleById(string vehicleId);
        Task<List<VehicleResponseDto>> GetVehicles(VehicleFilterDto filter);
        Task<List<VehicleResponseDto>> GetAssignedVehicleList(VehicleStatus Status);
        Task<List<VehicleResponseDto>> GetUnAssignedVehicleList(VehicleStatus Status);
        Task<bool> IsDriverAlreadyAssignedToSameVehicle(string vehicleId, string driverId);
        Task<bool> IsDriverAlreadyAssignedToAnotherVehicle(string vehicleId, string driverId);
        Task<bool> IsVehicleActive(string vehicleId);

        Task<ApiResponse<VehicleResponseDto>> ChangeStatusOfVehicle(string vehicleId, VehicleStatus status);
        Task <ApiResponse<VehicleResponseDto>> GetDocumentOfVehicleById(string vehicleId);

        Task<IEnumerable<VehicleResponseDto>> GetVehicleStatusAsync(VehicleStatus vehicleStatus);

    }
}
