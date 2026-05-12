using LogisticsERP.API.enums;

namespace LogisticsERP.API.DTOs.Vehicle
{
    public class VehicleFilterDto
    {
            public string? Number { get; set; }
            public string? Company { get; set; }
            public string? Doner { get; set; }
            public string? InsuredBy { get; set; }
            public string? DriverId { get; set; }
            public VehicleStatus? Status { get; set; }
            public string? Type { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
    }
}
