using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class ChargeLog_Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ChargeLogs");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ChargeLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompany",
                table: "ChargeLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeLogs_CompanyId",
                table: "ChargeLogs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeLogs_Companys_CompanyId",
                table: "ChargeLogs",
                column: "CompanyId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeLogs_Companys_CompanyId",
                table: "ChargeLogs");

            migrationBuilder.DropIndex(
                name: "IX_ChargeLogs_CompanyId",
                table: "ChargeLogs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ChargeLogs");

            migrationBuilder.DropColumn(
                name: "IsCompany",
                table: "ChargeLogs");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ChargeLogs",
                nullable: true);
        }
    }
}
