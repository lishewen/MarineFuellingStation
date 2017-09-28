using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class ChargeLog_ClientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "ChargeLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeLogs_ClientId",
                table: "ChargeLogs",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeLogs_Clients_ClientId",
                table: "ChargeLogs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeLogs_Clients_ClientId",
                table: "ChargeLogs");

            migrationBuilder.DropIndex(
                name: "IX_ChargeLogs_ClientId",
                table: "ChargeLogs");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ChargeLogs");
        }
    }
}
