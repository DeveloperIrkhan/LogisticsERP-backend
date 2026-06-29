using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsERP.API.Migrations
{
    /// <inheritdoc />
    public partial class Salaryfieldisremoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Drivers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salary",
                table: "Drivers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
