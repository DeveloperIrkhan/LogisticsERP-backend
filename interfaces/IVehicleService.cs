using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IVehicleService
    {
        Task<VehicleResponseDto> CreateVehicle(VehicleCreateDto vehicle);
        Task<VehicleResponseDto> UpdateVehicle(VehicleUpdateDto vehicle);
        Task DeleteVehicle(string id);
        Task<VehicleResponseDto> GetVehicleById(string id);
        Task<IEnumerable<VehicleResponseDto>> GetAllVehicles();
        Task<List<VehicleResponseDto>> GetExpiringVehicles(int days);
        Task<VehicleResponseDto> GetFullRecordByVehicleById(string id);
    }
}
