using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceHelper.Infrastructure.data.migrations
{
    public partial class nullbelContactInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersInfo_ContactsInfo_ContactInfoId",
                table: "UsersInfo");

            migrationBuilder.AlterColumn<int>(
                name: "ContactInfoId",
                table: "UsersInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInfo_ContactsInfo_ContactInfoId",
                table: "UsersInfo",
                column: "ContactInfoId",
                principalTable: "ContactsInfo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersInfo_ContactsInfo_ContactInfoId",
                table: "UsersInfo");

            migrationBuilder.AlterColumn<int>(
                name: "ContactInfoId",
                table: "UsersInfo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInfo_ContactsInfo_ContactInfoId",
                table: "UsersInfo",
                column: "ContactInfoId",
                principalTable: "ContactsInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
