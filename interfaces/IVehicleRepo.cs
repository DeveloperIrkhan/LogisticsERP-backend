using LogisticsERP.API.DTOs.Vehicle;

namespace LogisticsERP.API.interfaces
{
    public interface IVehicleRepo
    {
        Task<VehicleResponseDto> GetVehicleById(string vehicleId);
    }
}
