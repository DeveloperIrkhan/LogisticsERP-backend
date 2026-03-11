using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;

namespace LogisticsERP.API.Models
{
    public class Vehicle
    {
        [Key]
        public string VehicleId { get; set; } = $"PRCS-VEH-{Guid.NewGuid()}";
        public string Number { get; set; }
        public string ModelName { get; set; }
        public string Company { get; set; }
        public string EnginNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string Type { get; set; }
        public string Doner { get; set; }
        public decimal PurchsedCast { get; set; }
        public decimal Depreciation { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime RegistrationExpiry { get; set; }
        public DateTime FitnessExpiry { get; set; }
        public string InsuredBy { get; set; }
        public DateTime InsuranceExpiry { get; set; }
        public string TypeOfInsurance { get; set; }

        public DateTime InsuranceFrom { get; set; }
        public DateTime InsuranceTo { get; set; }
        public VehicleStatus Status { get; set; }

        // Navigation property → Vehicle can have multiple Drivers
        //this is ICollection because it can hold multiple drivers, and we initialize it to an empty list to avoid null reference issues.
        public ICollection<Driver> Drivers { get; set; } = new List<Driver>();
        public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = [];
        public ICollection<FuelRecord> FuelRecords { get; set; } = [];
        public ICollection<DutyLogs> DutyLogs { get; set; } = [];
        public ICollection<Expense> Expanse { get; set; } = [];
        public ICollection<VehicleDocuments> Documents { get; set; } = [];

    }
}
