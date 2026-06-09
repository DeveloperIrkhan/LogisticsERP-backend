using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsERP.API.Migrations
{
    /// <inheritdoc />
    public partial class changedthemisspilledfieldnameinvehicletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Vehicles",
                newName: "VehicleType");

            migrationBuilder.RenameColumn(
                name: "EnginNumber",
                table: "Vehicles",
                newName: "EngineNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleType",
                table: "Vehicles",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "EngineNumber",
                table: "Vehicles",
                newName: "EnginNumber");
        }
    }
}
