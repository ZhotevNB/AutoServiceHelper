using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceHelper.Infrastructure.data.Migrations
{
    public partial class updateUsersInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AskToChangeRollManager",
                table: "UsersInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AskToChangeRollMechanic",
                table: "UsersInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AskToChangeRollManager",
                table: "UsersInfo");

            migrationBuilder.DropColumn(
                name: "AskToChangeRollMechanic",
                table: "UsersInfo");
        }
    }
}
