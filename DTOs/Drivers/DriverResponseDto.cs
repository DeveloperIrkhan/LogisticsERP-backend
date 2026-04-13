using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.DTOs.Drivers
{
    public class DriverResponseDto
    {
        public string DriverId { get; set; }
        public string FullName { get; set; }
        public string CNIC { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseExpiry { get; set; }
        public string TypeOfLicence { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Salary { get; set; }
        public DriverStatus Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public string VehicleId { get; set; }

        // Optional: include vehicle basic info
        public string? VehicleName { get; set; }
    }
}
