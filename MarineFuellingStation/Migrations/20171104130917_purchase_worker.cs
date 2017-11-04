using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class purchase_worker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Constructor",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Constructor",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Worker",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Worker",
                table: "Purchases");

            migrationBuilder.AddColumn<string>(
                name: "Constructor",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Constructor",
                table: "Orders",
                nullable: true);
        }
    }
}
