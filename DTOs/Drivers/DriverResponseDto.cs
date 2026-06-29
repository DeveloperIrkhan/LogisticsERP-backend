using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.DTOs.Drivers
{
    public class DriverResponseDto
    {
        public string DriverId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpiry { get; set; }
        public string TypeOfLicence { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public DriverStatus Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? PhotoUrl { get; set; }
        public string? LicenseUrl { get; set; }

        public string? VehicleId { get; set; }
    }
}
