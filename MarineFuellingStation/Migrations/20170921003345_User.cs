using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinPrice",
                table: "Orders",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Orders");
        }
    }
}
