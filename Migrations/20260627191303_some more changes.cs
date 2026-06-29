using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsERP.API.Migrations
{
    /// <inheritdoc />
    public partial class somemorechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeDuties_Drivers_DriverId",
                table: "OvertimeDuties");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeDuties_DutyLogs_DutyLogDutyId",
                table: "OvertimeDuties");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeDuties_Users_UserId",
                table: "OvertimeDuties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OvertimeDuties",
                table: "OvertimeDuties");

            migrationBuilder.RenameTable(
                name: "OvertimeDuties",
                newName: "OvertimeDuty");

            migrationBuilder.RenameIndex(
                name: "IX_OvertimeDuties_UserId",
                table: "OvertimeDuty",
                newName: "IX_OvertimeDuty_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OvertimeDuties_DutyLogDutyId",
                table: "OvertimeDuty",
                newName: "IX_OvertimeDuty_DutyLogDutyId");

            migrationBuilder.RenameIndex(
                name: "IX_OvertimeDuties_DriverId",
                table: "OvertimeDuty",
                newName: "IX_OvertimeDuty_DriverId");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "OvertimeDuty",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OvertimeDuty",
                table: "OvertimeDuty",
                column: "OvertimeDutyId");

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeDuty_Drivers_DriverId",
                table: "OvertimeDuty",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeDuty_DutyLogs_DutyLogDutyId",
                table: "OvertimeDuty",
                column: "DutyLogDutyId",
                principalTable: "DutyLogs",
                principalColumn: "DutyId");

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeDuty_Users_UserId",
                table: "OvertimeDuty",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeDuty_Drivers_DriverId",
                table: "OvertimeDuty");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeDuty_DutyLogs_DutyLogDutyId",
                table: "OvertimeDuty");

            migrationBuilder.DropForeignKey(
                name: "FK_OvertimeDuty_Users_UserId",
                table: "OvertimeDuty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OvertimeDuty",
                table: "OvertimeDuty");

            migrationBuilder.RenameTable(
                name: "OvertimeDuty",
                newName: "OvertimeDuties");

            migrationBuilder.RenameIndex(
                name: "IX_OvertimeDuty_UserId",
                table: "OvertimeDuties",
                newName: "IX_OvertimeDuties_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OvertimeDuty_DutyLogDutyId",
                table: "OvertimeDuties",
                newName: "IX_OvertimeDuties_DutyLogDutyId");

            migrationBuilder.RenameIndex(
                name: "IX_OvertimeDuty_DriverId",
                table: "OvertimeDuties",
                newName: "IX_OvertimeDuties_DriverId");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedBy",
                table: "OvertimeDuties",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OvertimeDuties",
                table: "OvertimeDuties",
                column: "OvertimeDutyId");

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeDuties_Drivers_DriverId",
                table: "OvertimeDuties",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeDuties_DutyLogs_DutyLogDutyId",
                table: "OvertimeDuties",
                column: "DutyLogDutyId",
                principalTable: "DutyLogs",
                principalColumn: "DutyId");

            migrationBuilder.AddForeignKey(
                name: "FK_OvertimeDuties_Users_UserId",
                table: "OvertimeDuties",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
