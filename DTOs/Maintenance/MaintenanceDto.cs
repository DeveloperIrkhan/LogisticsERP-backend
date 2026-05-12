using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.DTOs.Maintenance
{
    public class MaintenanceCreateDto
    {
        public string VehicleId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal CurrentKm { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime? NextServiceDate { get; set; }
        public string AddedBy { get; set; }
    }


    public class MaintenanceUpdateDto
    {
        public DateTime MaintenanceDate { get; set; }
        public decimal CurrentKm { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime? NextServiceDate { get; set; }
    }


    public class MaintenanceResponseDto
    {
        public string MaintenanceRecordId { get; set; }
        public string VehicleId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal CurrentKm { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime? NextServiceDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class MaintenanceSummaryDto
    {
        public string VehicleId { get; set; }
        public decimal TotalCost { get; set; }
        public int TotalRecords { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
    }


}
