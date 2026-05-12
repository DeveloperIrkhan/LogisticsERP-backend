using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IDriverService
    {
        public Task<ApiResponse<DriverResponseDto>> AssignDriver(string driverId, string vehicleId);
        public Task<List<DriverResponseDto>> DriverListAssignedToSpecficVehicle(string vehicleId);

        Task<DriverResponseDto> CreateDriver(DriverCreateDto driver);
        Task<DriverResponseDto> UpdateDriver(DriverUpdateDto driver);
        Task DeleteVehicle(string id);
        Task<DriverResponseDto> GetDriverById(string id);
        //Task<DriverResponseDto> ImageUploadingAsync(string FileUrl);
        Task<List<DriverResponseDto>> GetAssignedDriversListForSignleVehicle(string vehId);
        Task<IEnumerable<DriverResponseDto>> GetAllDrivers();
        Task<DriverResponseDto> UnassignDriver(string driverId);

    }
}
