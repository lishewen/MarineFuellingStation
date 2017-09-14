using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class Purchase_State : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssayId",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Scale",
                table: "Purchases",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ScalePic",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ScaleWithCar",
                table: "Purchases",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ScaleWithCarPic",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_AssayId",
                table: "Purchases",
                column: "AssayId",
                unique: true,
                filter: "[AssayId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Assays_AssayId",
                table: "Purchases",
                column: "AssayId",
                principalTable: "Assays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Assays_AssayId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_AssayId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "AssayId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Scale",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ScalePic",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ScaleWithCar",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ScaleWithCarPic",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Purchases");
        }
    }
}
