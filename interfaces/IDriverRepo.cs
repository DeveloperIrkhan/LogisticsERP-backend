using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IDriverRepo
    {
        Task<List<Driver>> GerDriversListForSpecficVehicle(string vehicleId);
        Task<List<Driver>> getAllDriverAsync();
        Task<List<Driver>> GetAvailableDriversAsync();
        Task<List<Driver>> GetDriversByStatusAsync(DriverStatus status);
        Task<bool> IsCNICDuplicateAsync(string cnic, string? excludeDriverId = null);
        Task<bool> IsLicenseDuplicateAsync(string licenseNumber, string? excludeDriverId = null);

    }
}