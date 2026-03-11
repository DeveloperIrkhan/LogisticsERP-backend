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
        
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<OvertimeDuty> OvertimeDuties { get; set; }
        public DbSet<Role> UserRole { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

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
                .Property(x => x.ExpenseType)
                .HasConversion<string>();
            modelBuilder.Entity<Expense>()
                .Property(x => x.PaymentMode)
                .HasConversion<string>();
            modelBuilder.Entity<Expense>()
                .Property(x => x.ExpenseStatus)
                .HasConversion<string>();


        }
    }


}

