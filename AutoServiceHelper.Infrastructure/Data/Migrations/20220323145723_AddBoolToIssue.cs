using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceHelper.Infrastructure.data.Migrations
{
    public partial class AddBoolToIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFixed",
                table: "Issues",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFixed",
                table: "Issues");
        }
    }
}
