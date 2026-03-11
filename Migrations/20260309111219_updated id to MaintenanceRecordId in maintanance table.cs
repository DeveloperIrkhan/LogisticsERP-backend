using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsERP.API.Migrations
{
    /// <inheritdoc />
    public partial class updatedidtoMaintenanceRecordIdinmaintanancetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MaintenanceRecords",
                newName: "MaintenanceRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaintenanceRecordId",
                table: "MaintenanceRecords",
                newName: "Id");
        }
    }
}
