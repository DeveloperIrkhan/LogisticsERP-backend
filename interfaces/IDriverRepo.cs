using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;

namespace LogisticsERP.API.interfaces
{
    public interface IDriverRepo
    {
        Task<List<DriverResponseDto>> GerDriverListForSpecficVehicle(string vehicleId);
        Task<List<DriverResponseDto>> getAllDriverAsync();
    }
}