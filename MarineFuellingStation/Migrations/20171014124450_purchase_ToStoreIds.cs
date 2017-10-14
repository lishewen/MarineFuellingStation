using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class purchase_ToStoreIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToStoreCounts",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToStoreIds",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToStoreNames",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToStoreCounts",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ToStoreIds",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ToStoreNames",
                table: "Purchases");
        }
    }
}
