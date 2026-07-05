using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;

namespace LogisticsERP.API.DTOs.Drivers
{
    public class DriverUpdateDto
    {
        public string? DriverId { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? CNIC { get; set; } 
        public DateTime CnicExpiry { get; set; }
        public string? MobileNumber { get; set; } 
        public string? Email { get; set; } 
        public string? Address { get; set; } 
        public string? LicenseNumber { get; set; } 
        public DateTime? LicenseExpiry { get; set; }
        public string? TypeOfLicence { get; set; } 
        public DateTime? DateOfJoining { get; set; }

        public IFormFile? Photo { get; set; }
        public IFormFile? License { get; set; }
        public DriverStatus? Status { get; set; }
        public string? Description { get; set; } 

        public string? VehicleId { get; set; }
    }
}
