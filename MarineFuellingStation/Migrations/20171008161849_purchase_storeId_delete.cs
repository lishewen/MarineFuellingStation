using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class purchase_storeId_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Stores_StoreId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_AssayId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_StoreId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Purchases");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_AssayId",
                table: "Purchases",
                column: "AssayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Purchases_AssayId",
                table: "Purchases");

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Purchases",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_AssayId",
                table: "Purchases",
                column: "AssayId",
                unique: true,
                filter: "[AssayId] IS NOT NULL");

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
    }
}
