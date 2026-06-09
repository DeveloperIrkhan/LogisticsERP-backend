using LogisticsERP.API.enums;
using LogisticsERP.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.DTOs.Drivers
{
    public class DriverCreateDto
    {
        public string FullName { get; set; }
        public string CNIC { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string LicenseNumber { get; set; }
        public IFormFile? Photo { get; set; }
        public IFormFile? License { get; set; }

        public DateTime LicenseExpiry { get; set; } = DateTime.UtcNow;
        public string TypeOfLicence { get; set; }
        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;
        public string Salary { get; set; }
        public DriverStatus Status { get; set; }
        public string Description { get; set; }
        public string? VehicleId { get; set; }
    }
}
