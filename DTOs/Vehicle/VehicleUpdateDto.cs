using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;

namespace LogisticsERP.API.DTOs.Vehicle
{
    public class VehicleUpdateDto
    {
        [Required]
        public string VehicleId { get; set; } = string.Empty;

        public string Number { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string EngineNumber { get; set; } = string.Empty;
        public string ChassisNumber { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string Doner { get; set; } = string.Empty;
        public decimal? PurchsedCast { get; set; }  
        public decimal? Depreciation { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? RegistrationExpiry { get; set; }
        public DateTime? FitnessExpiry { get; set; }
        public string InsuredBy { get; set; } = string.Empty;
        public DateTime? InsuranceFrom { get; set; }
        public DateTime? InsuranceTo { get; set; }
        public string TypeOfInsurance { get; set; } = string.Empty;
        public VehicleStatus? Status { get; set; }
    }
}
