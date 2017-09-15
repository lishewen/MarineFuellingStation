using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class Order_OilCarWeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OilCarPic",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OilCarWeightPic",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OilCarWeightPic",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OilCarPic",
                table: "Orders",
                nullable: true);
        }
    }
}
