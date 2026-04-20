using LogisticsERP.API.DTOs.Documents;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.DTOs.Vehicle
{
    public class VehicleResponseDto
    {
        public string VehicleId { get; set; }
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

        public List<DriverResponseDto> Drivers { get; set; } = [];
        public List<DocumentResponseDto> Documents { get; set; } = [];
        //public List<> MaintenanceRecords { get; set; } = [];
        //public List<FuelRecord> FuelRecords { get; set; } = [];
        //public List<DutyLogs> DutyLogs { get; set; } = [];
        //public List<Expense> Expanse { get; set; } = [];
    }
}
