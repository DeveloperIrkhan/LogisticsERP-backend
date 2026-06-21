using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;

namespace LogisticsERP.API.DTOs.Vehicle
{
    public class VehicleCreateDto
    {
        public string Number { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string EngineNumber { get; set; } = string.Empty;
        public string ChassisNumber { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string Doner { get; set; } = string.Empty;
        public decimal PurchsedCast { get; set; } 
        public decimal Depreciation { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public DateTime RegistrationExpiry { get; set; } = DateTime.UtcNow;
        public DateTime FitnessExpiry { get; set; } = DateTime.UtcNow;
        public string InsuredBy { get; set; } = string.Empty;
        public DateTime InsuranceFrom { get; set; } = DateTime.UtcNow;
        public DateTime InsuranceExpiry { get; set; } = DateTime.UtcNow;
        public DateTime InsuranceTo { get; set; } = DateTime.UtcNow;
        public string TypeOfInsurance { get; set; } = string.Empty;
        public VehicleStatus Status { get; set; }
    }
}
