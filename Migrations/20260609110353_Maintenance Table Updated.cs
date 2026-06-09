using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsERP.API.Migrations
{
    /// <inheritdoc />
    public partial class MaintenanceTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "NextServiceDate",
                table: "MaintenanceRecords");

            migrationBuilder.AddColumn<string>(
                name: "ChangedParts",
                table: "MaintenanceRecords",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "MaintenanceRecords",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaintenanceType",
                table: "MaintenanceRecords",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkshopName",
                table: "MaintenanceRecords",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StationName",
                table: "FuelRecords",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "ExpenseType",
                table: "Expenses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedParts",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "MaintenanceType",
                table: "MaintenanceRecords");

            migrationBuilder.DropColumn(
                name: "WorkshopName",
                table: "MaintenanceRecords");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MaintenanceRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NextServiceDate",
                table: "MaintenanceRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "StationName",
                table: "FuelRecords",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ExpenseType",
                table: "Expenses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
