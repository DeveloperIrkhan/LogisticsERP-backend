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
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemPurchase> ItemPurchases { get; set; }
        public DbSet<ItemSale> ItemSales { get; set; }
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



            // ── Items / Purchases / Sales module ────────────────────

            modelBuilder.Entity<Item>()
                .Property(x => x.ItemCategory)
                .HasConversion<string>();
            modelBuilder.Entity<Item>()
                .Property(x => x.Unit)
                .HasConversion<string>();
            modelBuilder.Entity<Item>()
                .Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<ItemPurchase>()
                .Property(x => x.PaymentMode)
                .HasConversion<string>();
            modelBuilder.Entity<ItemPurchase>()
                .Property(x => x.Status)
                .HasConversion<string>();
            modelBuilder.Entity<ItemPurchase>()
                .Property(x => x.PurchaseDate)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<ItemPurchase>()
                .Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<ItemPurchase>()
                .HasOne(x => x.Item)
                .WithMany(i => i.ItemPurchase)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ItemPurchase>()
                .HasOne(x => x.Vehicle)
                .WithMany()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<ItemPurchase>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.AddedBy)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ItemSale>()
                .Property(x => x.PaymentMode)
                .HasConversion<string>();
            modelBuilder.Entity<ItemSale>()
                .Property(x => x.Status)
                .HasConversion<string>();
            modelBuilder.Entity<ItemSale>()
                .Property(x => x.SaleDate)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<ItemSale>()
                .Property(x => x.CreatedAt)
                .HasColumnType("timestamp with time zone");
            modelBuilder.Entity<ItemSale>()
                .HasOne(x => x.Item)
                .WithMany(i => i.Sales)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ItemSale>()
                .HasOne(x => x.Vehicle)
                .WithMany()
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<ItemSale>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.AddedBy)
                .OnDelete(DeleteBehavior.SetNull);


        }
    }


}

