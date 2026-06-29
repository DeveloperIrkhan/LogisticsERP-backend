using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;
using System.Collections.Specialized;
using System.Net.NetworkInformation;

namespace LogisticsERP.API.interfaces
{
    public interface IVehicleRepo
    {
        Task<Vehicle> GetVehicleById(string vehicleId);
        Task<List<Vehicle>> GetVehicles(VehicleFilterDto filter);
        Task<List<Vehicle>> GetAssignedVehicleList(VehicleStatus Status);
        Task<List<Vehicle>> GetUnAssignedVehicleList(VehicleStatus Status);
        Task<bool> IsDriverAlreadyAssignedToSameVehicle(string vehicleId, string driverId);
        Task<bool> IsDriverAlreadyAssignedToAnotherVehicle(string vehicleId, string driverId);
        Task<bool> IsVehicleActive(string vehicleId);
        Task<List<Vehicle>> GetAllVehiclesAsync();

        Task<Vehicle?> ChangeStatusOfVehicle(string vehicleId, VehicleStatus status);
        Task <Vehicle?> GetDocumentOfVehicleById(string vehicleId);

        Task<IEnumerable<Vehicle>> GetVehicleStatusAsync(VehicleStatus vehicleStatus);

    }
}
