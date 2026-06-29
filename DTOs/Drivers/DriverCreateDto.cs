using LogisticsERP.API.enums;
using LogisticsERP.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.DTOs.Drivers
{
    public class DriverCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
        public IFormFile? License { get; set; } 

        public DateTime LicenseExpiry { get; set; } = DateTime.UtcNow;
        public string TypeOfLicence { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;
        public DriverStatus Status { get; set; }    
        public string Description { get; set; } = string.Empty;
        public string? VehicleId { get; set; }
    }
}
