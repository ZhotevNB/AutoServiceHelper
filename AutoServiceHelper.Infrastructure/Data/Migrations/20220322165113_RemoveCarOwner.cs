using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceHelper.Infrastructure.data.Migrations
{
    public partial class RemoveCarOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarOwners_UserId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "CarOwners");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_AspNetUsers_UserId",
                table: "Cars",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_AspNetUsers_UserId",
                table: "Cars");

            migrationBuilder.CreateTable(
                name: "CarOwners",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarOwners", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CarOwners_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarOwners_UserId",
                table: "Cars",
                column: "UserId",
                principalTable: "CarOwners",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
