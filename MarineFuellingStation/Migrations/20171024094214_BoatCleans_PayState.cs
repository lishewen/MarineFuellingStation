using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class BoatCleans_PayState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoatCleanId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayState",
                table: "BoatCleans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BoatCleanId",
                table: "Payments",
                column: "BoatCleanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_BoatCleans_BoatCleanId",
                table: "Payments",
                column: "BoatCleanId",
                principalTable: "BoatCleans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_BoatCleans_BoatCleanId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_BoatCleanId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BoatCleanId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PayState",
                table: "BoatCleans");
        }
    }
}
