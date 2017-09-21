using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class Purchase_storeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_StoreId",
                table: "Purchases",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Stores_StoreId",
                table: "Purchases",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Stores_StoreId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_StoreId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Purchases");
        }
    }
}
