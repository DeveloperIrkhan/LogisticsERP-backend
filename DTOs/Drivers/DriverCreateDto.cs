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
        public IFormFile? Photo { get; set; } = null;
        public string? PhotoUrl { get; set; } = string.Empty;
        public DateTime LicenseExpiry { get; set; } = DateTime.UtcNow;
        public string typeOfLicence { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;
        public string Salary { get; set; } = string.Empty;
        public DriverStatus Status { get; set; }
        public string Description { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
    }
}
