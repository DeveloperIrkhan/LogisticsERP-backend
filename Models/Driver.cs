using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class Driver
    {
        [Key] 
        public string DriverId { get; set; } = $"PRCS-DRV-{Guid.NewGuid()}";
        public string FullName { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public DateTime CnicExpiry { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? LicenseUrl { get; set; }
        public DateTime LicenseExpiry { get; set; }
        public string typeOfLicence { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;
        public DriverStatus Status { get; set; } = DriverStatus.Active;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //FORIGN KEY TABLE 
        // Navigation property for the related Vehicle
        // Nullable, optional assignment
        public string? VehicleId { get; set; }
        [ForeignKey("VehicleId")] 
        public Vehicle? Vehicle { get; set; }
        public ICollection<FuelRecord>? FuelRecords { get; set; } = [];
        public ICollection<MaintenanceRecord>? MaintenanceRecords { get; set; } = [];
        public ICollection<DutyLogs>? DutyLogs { get; set; } = [];
        public ICollection<OvertimeDuty>? OvertimeDuties { get; set; } = [];
    }
}
