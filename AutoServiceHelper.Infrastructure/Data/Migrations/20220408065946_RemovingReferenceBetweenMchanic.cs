using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceHelper.Infrastructure.data.migrations
{
    public partial class RemovingReferenceBetweenMchanic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MechanicActivity_Mechanics_UserId",
                table: "MechanicActivity");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MechanicActivity",
                newName: "MechanicId");

            migrationBuilder.RenameIndex(
                name: "IX_MechanicActivity_UserId",
                table: "MechanicActivity",
                newName: "IX_MechanicActivity_MechanicId");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicActivity_AspNetUsers_MechanicId",
                table: "MechanicActivity",
                column: "MechanicId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MechanicActivity_AspNetUsers_MechanicId",
                table: "MechanicActivity");

            migrationBuilder.RenameColumn(
                name: "MechanicId",
                table: "MechanicActivity",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MechanicActivity_MechanicId",
                table: "MechanicActivity",
                newName: "IX_MechanicActivity_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicActivity_Mechanics_UserId",
                table: "MechanicActivity",
                column: "UserId",
                principalTable: "Mechanics",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
