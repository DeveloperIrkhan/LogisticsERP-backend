namespace LogisticsERP.API.DTOs.Drivers
{
    public class DriverDutyStatsDto
    {
        public string DriverId { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public int TotalDuties { get; set; }
        public int CompletedDuties { get; set; }
        public int MissedDuties { get; set; }
        public int CancelledDuties { get; set; }
        public int CurrentlyOnDuty { get; set; }
        public decimal TotalKmDriven { get; set; }
        public decimal TotalHours { get; set; }
        public DateTime? LastDutyDate { get; set; }
        public bool IsAvailable { get; set; }
    }
}
