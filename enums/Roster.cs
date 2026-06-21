namespace LogisticsERP.API.enums
{
    public enum RosterStatus
    {
        Draft,
        Published,
        Approved,
        Closed
    }

    public enum ShiftType
    {
        Morning,    // 08:00 - 16:00
        Evening,    // 16:00 - 00:00
        Night,      // 00:00 - 08:00
        FullDay,    // 08:00 - 20:00
        Custom      // user defined times
    }

    public enum RosterEntryStatus
    {
        Scheduled,    // planned, not yet executed
        InProgress,   // driver on duty right now
        Completed,    // duty done, DutyLog linked
        Missed,       // driver didn't show up
        Cancelled     // cancelled before duty
    }
}
