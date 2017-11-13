using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class order_isPrintPrice_isDeliver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeliver",
                table: "SalesPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrintPrice",
                table: "SalesPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "SalesPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliver",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrintPrice",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeliver",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "IsPrintPrice",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "IsDeliver",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsPrintPrice",
                table: "Orders");
        }
    }
}
