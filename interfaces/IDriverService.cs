using LogisticsERP.API.DTOs.Drivers;

namespace LogisticsERP.API.interfaces
{
    public interface IDriverService
    {
        public Task<DriverResponseDto> AssignDriver(string vehicleId, string driverId);
        public Task<List<DriverResponseDto>> DriverListAssignedToSpecficVehicle(string vehicleId);

        Task<DriverResponseDto> CreateDriver(DriverCreateDto driver);
        Task<DriverResponseDto> UpdateDriver(DriverUpdateDto driver);
        Task DeleteVehicle(string id);
        Task<DriverResponseDto> GetDriverById(string id);
        Task<IEnumerable<DriverResponseDto>> GetAllDrivers();
    }
}
