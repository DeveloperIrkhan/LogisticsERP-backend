using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsERP.API.Migrations
{
    /// <inheritdoc />
    public partial class vehiclemodelsandlogchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentKm",
                table: "MaintenanceRecords",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextMaintenanceDate",
                table: "MaintenanceRecords",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NextMaintenanceKm",
                table: "MaintenanceRecords",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VehicleDocuments",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "text", nullable: false),
                    VehicleId = table.Column<string>(type: "text", nullable: false),
                    DocumentType = table.Column<string>(type: "text", nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: false),
                    PublicId = table.Column<string>(type: "text", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDocuments", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_VehicleDocuments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDocuments_VehicleId",
                table: "VehicleDocuments",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleDocuments");

            migrationBuilder.DropColumn(
                name: "CurrentKm",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "NextMaintenanceDate",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "NextMaintenanceKm",
                table: "MaintenanceRecords");
        }
    }
}
