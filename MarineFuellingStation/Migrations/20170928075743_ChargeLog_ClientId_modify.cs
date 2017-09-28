using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class ChargeLog_ClientId_modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeLogs_Clients_ClientId",
                table: "ChargeLogs");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "ChargeLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeLogs_Clients_ClientId",
                table: "ChargeLogs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChargeLogs_Clients_ClientId",
                table: "ChargeLogs");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "ChargeLogs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeLogs_Clients_ClientId",
                table: "ChargeLogs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
