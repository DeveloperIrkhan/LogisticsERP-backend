using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsERP.API.Migrations
{
    /// <inheritdoc />
    public partial class modelchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mileage",
                table: "FuelRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Donor",
                table: "DutyLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "KillometerIn",
                table: "DutyLogs",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "KillometerOut",
                table: "DutyLogs",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficerName",
                table: "DutyLogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "DutyLogs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "FuelRecords");

            migrationBuilder.DropColumn(
                name: "Donor",
                table: "DutyLogs");

            migrationBuilder.DropColumn(
                name: "KillometerIn",
                table: "DutyLogs");

            migrationBuilder.DropColumn(
                name: "KillometerOut",
                table: "DutyLogs");

            migrationBuilder.DropColumn(
                name: "OfficerName",
                table: "DutyLogs");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "DutyLogs");
        }
    }
}
