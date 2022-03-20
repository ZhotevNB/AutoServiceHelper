using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceHelper.Infrastructure.data.migrations
{
    public partial class ChangingTablesIssueOrderCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Oddometer",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "CarOdometer",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarOdometer",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarOdometer",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CarOdometer",
                table: "Issues");

            migrationBuilder.AddColumn<string>(
                name: "Oddometer",
                table: "Cars",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
