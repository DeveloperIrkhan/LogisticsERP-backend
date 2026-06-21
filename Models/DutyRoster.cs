using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;

namespace LogisticsERP.API.Models
{
    public class DutyRoster
    {
        [Key]
        public string RosterId { get; set; } = $"PRCS-RST-{Guid.NewGuid()}";

        public string Title { get; set; } = string.Empty;      // e.g. "June 2026 Roster"
        public int Month { get; set; }
        public int Year { get; set; }
        public RosterStatus Status { get; set; } = RosterStatus.Draft;
        public string? CreatedBy { get; set; }
        public string? ApprovedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public List<DutyRosterEntry> Entries { get; set; } = new();
    }
}
