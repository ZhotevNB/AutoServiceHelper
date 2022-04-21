using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceHelper.Infrastructure.data.Migrations
{
    public partial class OfferEntetyBugFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_AutoShops_AutoShopId",
                table: "Mechanics");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_ShopServices_ShopServiceId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Offer",
                table: "Orders");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShopServiceId",
                table: "Parts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MechanicId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "AutoShopId",
                table: "Mechanics",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OfferId",
                table: "Orders",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_AutoShops_AutoShopId",
                table: "Mechanics",
                column: "AutoShopId",
                principalTable: "AutoShops",
                principalColumn: "Id");
            //important: the onDelete is changed to restrict manual
            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Offers_OfferId",
                table: "Orders",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_ShopServices_ShopServiceId",
                table: "Parts",
                column: "ShopServiceId",
                principalTable: "ShopServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mechanics_AutoShops_AutoShopId",
                table: "Mechanics");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Offers_OfferId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_ShopServices_ShopServiceId",
                table: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OfferId",
                table: "Orders");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShopServiceId",
                table: "Parts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "MechanicId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Offer",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "AutoShopId",
                table: "Mechanics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanics_AutoShops_AutoShopId",
                table: "Mechanics",
                column: "AutoShopId",
                principalTable: "AutoShops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_ShopServices_ShopServiceId",
                table: "Parts",
                column: "ShopServiceId",
                principalTable: "ShopServices",
                principalColumn: "Id");
        }
    }
}
