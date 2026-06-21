using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DutyLogs> DutyLogs { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<FuelRecord> FuelRecords { get; set; }
        public DbSet<VehicleDocuments> VehicleDocuments { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<OvertimeDuty> OvertimeDuties { get; set; }
        public DbSet<Role> UserRole { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<DutyRoster> DutyRosters { get; set; }
        public DbSet<DutyRosterEntry> DutyRosterEntries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>()
                 .HasOne(d => d.Vehicle)
                 .WithMany(e => e.Drivers)
                 .HasForeignKey(d => d.VehicleId)
                 .OnDelete(DeleteBehavior.SetNull);

            // Configure enum properties to be stored as strings
            modelBuilder.Entity<User>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Driver>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Vehicle>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Expense>()
                .Property(x => x.PaymentMode)
                .HasConversion<string>();

            modelBuilder.Entity<Expense>()
                .Property(x => x.ExpenseStatus)
                .HasConversion<string>();

            modelBuilder.Entity<DutyLogs>()
                .Property(x => x.DutyType)
                .HasConversion<string>();

            modelBuilder.Entity<DutyLogs>()
                .Property(y => y.Status)
                .HasConversion<string>();
            
            modelBuilder.Entity<OvertimeDuty>()
                .Property(y => y.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Expense>()
                .Property(x => x.ExpenseCategory)
                .HasConversion<string>();
            modelBuilder.Entity<DutyRoster>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<DutyRosterEntry>()
                .Property(x => x.ShiftType)
                .HasConversion<string>();
            modelBuilder.Entity<DutyRosterEntry>()
                .Property(x => x.DutyType)
                .HasConversion<string>();
            modelBuilder.Entity<DutyRosterEntry>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<MaintenanceRecord>()
                 .Property(x => x.MaintenanceDate)
                 .HasColumnType("timestamp with time zone");


            modelBuilder.Entity<MaintenanceRecord>()
                .Property(x => x.NextMaintenanceDate)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<FuelRecord>()
                .Property(x => x.FuelingDate)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<FuelRecord>()
                .Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<DutyLogs>()
                .Property(x => x.DateOut)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<DutyLogs>()
                .Property(x => x.DateIn)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<DutyLogs>()
                .Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<DutyLogs>()
                .Property(x => x.CancelledAt)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<OvertimeDuty>()
                .Property(x => x.Date)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<OvertimeDuty>()
                .Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Driver>()
                .Property(x => x.LicenseExpiry)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<Driver>()
                .Property(x => x.DateOfJoining)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<Driver>()
                .Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone");

        }
    }


}

