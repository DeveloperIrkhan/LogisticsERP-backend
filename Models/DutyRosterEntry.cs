using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class DutyRosterEntry
    {
        [Key]
        public string EntryId { get; set; } = $"PRCS-REN-{Guid.NewGuid()}";

        public string RosterId { get; set; } = string.Empty;
        [ForeignKey("RosterId")]
        public DutyRoster Roster { get; set; }

        public string DriverId { get; set; } = string.Empty;
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }

        public string VehicleId { get; set; } = string.Empty;
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }

        public DateTime DutyDate { get; set; }           // specific day
        public ShiftType ShiftType { get; set; }         // Morning, Evening, Night, Custom
        public TimeSpan ShiftStart { get; set; }         // e.g. 08:00
        public TimeSpan ShiftEnd { get; set; }           // e.g. 16:00
        public DutyType DutyType { get; set; }           // Routine, Special, Occasion
        public string? Purpose { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public string? OfficerName { get; set; }
        public string? Donor { get; set; }
        public string? Notes { get; set; }
        public RosterEntryStatus Status { get; set; } = RosterEntryStatus.Scheduled;

        // Link to actual DutyLog when duty is executed
        public string? DutyLogId { get; set; }
        [ForeignKey("DutyLogId")]
        public DutyLogs? DutyLog { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
