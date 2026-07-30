namespace LogisticsERP.API.Helpers
{
    // Role is a DB-driven table (Models/Role.cs) so an Admin can add more roles later,
    // but these five are seeded on startup and used with [Authorize(Roles = "...")].
    public class RoleNames
    {
        public const string Admin = "Admin";
        public const string FleetManager = "FleetManager";
        public const string DataEntryOperator = "DataEntryOperator";
        public const string Driver = "Driver";
        public const string Viewer = "Viewer";

        public static readonly string[] All =
            [
            Admin, FleetManager, DataEntryOperator, Driver
            ];

        // Role every public sign-up gets by default until an Admin reviews it on approval.
        public const string DefaultSignupRole = DataEntryOperator;
    }
}
