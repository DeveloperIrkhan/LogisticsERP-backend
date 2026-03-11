using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsERP.API.Migrations
{
    /// <inheritdoc />
    public partial class updatedidfrominttostrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false),
                    ModelName = table.Column<string>(type: "text", nullable: false),
                    Company = table.Column<string>(type: "text", nullable: false),
                    EnginNumber = table.Column<string>(type: "text", nullable: false),
                    ChassisNumber = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Doner = table.Column<string>(type: "text", nullable: false),
                    PurchsedCast = table.Column<decimal>(type: "numeric", nullable: false),
                    Depreciation = table.Column<decimal>(type: "numeric", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RegistrationExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FitnessExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InsuredBy = table.Column<string>(type: "text", nullable: false),
                    InsuranceExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TypeOfInsurance = table.Column<string>(type: "text", nullable: false),
                    InsuranceFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InsuranceTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_UserRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    CNIC = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    MobileNumber = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    LicenseNumber = table.Column<string>(type: "text", nullable: false),
                    LicenseExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    typeOfLicence = table.Column<string>(type: "text", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Salary = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VehicleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                    table.ForeignKey(
                        name: "FK_Drivers_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseId = table.Column<string>(type: "text", nullable: false),
                    ExpenseName = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpenseType = table.Column<string>(type: "text", nullable: false),
                    PaymentMode = table.Column<string>(type: "text", nullable: false),
                    ExpenseStatus = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    VehicleId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expenses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "DutyLogs",
                columns: table => new
                {
                    DutyId = table.Column<string>(type: "text", nullable: false),
                    VehicleId = table.Column<string>(type: "text", nullable: false),
                    DriverId = table.Column<string>(type: "text", nullable: false),
                    FromLocation = table.Column<string>(type: "text", nullable: false),
                    ToLocation = table.Column<string>(type: "text", nullable: false),
                    DateOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TotalHours = table.Column<decimal>(type: "numeric", nullable: true),
                    ApprovedBy = table.Column<int>(type: "integer", nullable: true),
                    ApprovedByUserUserId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyLogs", x => x.DutyId);
                    table.ForeignKey(
                        name: "FK_DutyLogs_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DutyLogs_Users_ApprovedByUserUserId",
                        column: x => x.ApprovedByUserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_DutyLogs_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelRecords",
                columns: table => new
                {
                    FuelId = table.Column<string>(type: "text", nullable: false),
                    VehicleId = table.Column<string>(type: "text", nullable: false),
                    DriverId = table.Column<string>(type: "text", nullable: false),
                    FuelingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OdoMeterReading = table.Column<int>(type: "integer", nullable: false),
                    Liters = table.Column<decimal>(type: "numeric", nullable: false),
                    CostPerLiter = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: false),
                    StationName = table.Column<decimal>(type: "numeric", nullable: false),
                    StationLocation = table.Column<string>(type: "text", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "text", nullable: false),
                    Donor = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelRecords", x => x.FuelId);
                    table.ForeignKey(
                        name: "FK_FuelRecords_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuelRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    VehicleId = table.Column<string>(type: "text", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    NextServiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AddedBy = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DriverId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId");
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OvertimeDuties",
                columns: table => new
                {
                    OvertimeDutyId = table.Column<string>(type: "text", nullable: false),
                    DriverId = table.Column<string>(type: "text", nullable: false),
                    DutyId = table.Column<string>(type: "text", nullable: true),
                    DutyLogDutyId = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Hours = table.Column<decimal>(type: "numeric", nullable: false),
                    RatePerHour = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    ApprovedBy = table.Column<int>(type: "integer", nullable: false),
                    ApprovedByUserUserId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeDuties", x => x.OvertimeDutyId);
                    table.ForeignKey(
                        name: "FK_OvertimeDuties_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OvertimeDuties_DutyLogs_DutyLogDutyId",
                        column: x => x.DutyLogDutyId,
                        principalTable: "DutyLogs",
                        principalColumn: "DutyId");
                    table.ForeignKey(
                        name: "FK_OvertimeDuties_Users_ApprovedByUserUserId",
                        column: x => x.ApprovedByUserUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_VehicleId",
                table: "Drivers",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyLogs_ApprovedByUserUserId",
                table: "DutyLogs",
                column: "ApprovedByUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyLogs_DriverId",
                table: "DutyLogs",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DutyLogs_VehicleId",
                table: "DutyLogs",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_VehicleId",
                table: "Expenses",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelRecords_DriverId",
                table: "FuelRecords",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelRecords_VehicleId",
                table: "FuelRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_DriverId",
                table: "MaintenanceRecords",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_UserId",
                table: "MaintenanceRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_VehicleId",
                table: "MaintenanceRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeDuties_ApprovedByUserUserId",
                table: "OvertimeDuties",
                column: "ApprovedByUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeDuties_DriverId",
                table: "OvertimeDuties",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_OvertimeDuties_DutyLogDutyId",
                table: "OvertimeDuties",
                column: "DutyLogDutyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "FuelRecords");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "OvertimeDuties");

            migrationBuilder.DropTable(
                name: "DutyLogs");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
