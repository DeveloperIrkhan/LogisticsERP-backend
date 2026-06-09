using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.DTOs.Maintenance
{
    // ─── CREATE ───────────────────────────────────────────────
    public class MaintenanceCreateDto
    {
        public string VehicleId { get; set; }
        public string AddedBy { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal CurrentKm { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string? MaintenanceType { get; set; }
        public string? WorkshopName { get; set; }
        public string? ChangedParts { get; set; }
        public string? InvoiceNumber { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
    }

    // ─── UPDATE ───────────────────────────────────────────────
    public class MaintenanceUpdateDto
    {
        public DateTime? MaintenanceDate { get; set; }
        public decimal? CurrentKm { get; set; }
        public decimal? Cost { get; set; }
        public string? Description { get; set; }
        public string? MaintenanceType { get; set; }
        public string? WorkshopName { get; set; }
        public string? ChangedParts { get; set; }
        public string? InvoiceNumber { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
    }

    // ─── RESPONSE ─────────────────────────────────────────────
    public class MaintenanceResponseDto
    {
        public string MaintenanceRecordId { get; set; }
        public string VehicleId { get; set; }
        public string AddedBy { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public decimal CurrentKm { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string? MaintenanceType { get; set; }
        public string? WorkshopName { get; set; }
        public string? ChangedParts { get; set; }
        public string? InvoiceNumber { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── SUMMARY (per vehicle) ─────────────────────────────────
    public class MaintenanceSummaryDto
    {
        public string VehicleId { get; set; }
        public decimal TotalCost { get; set; }
        public int TotalRecords { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
    }

    // ─── COST REPORT ──────────────────────────────────────────
    public class MaintenanceCostReportDto
    {
        public int Year { get; set; }
        public int? Month { get; set; }
        public decimal TotalCost { get; set; }
        public int TotalRecords { get; set; }
    }


}
