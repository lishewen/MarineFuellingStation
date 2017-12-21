using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class IsDel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "SalesPlans",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Purchases",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "MoveStores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "BoatCleans",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "MoveStores");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "BoatCleans");
        }
    }
}
