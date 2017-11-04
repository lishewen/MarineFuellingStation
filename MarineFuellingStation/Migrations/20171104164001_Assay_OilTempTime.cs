using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class Assay_OilTempTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OilTempTime",
                table: "Assays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "凝点",
                table: "Assays",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "十六烷指数",
                table: "Assays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "含硫",
                table: "Assays",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "蚀点",
                table: "Assays",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OilTempTime",
                table: "Assays");

            migrationBuilder.DropColumn(
                name: "凝点",
                table: "Assays");

            migrationBuilder.DropColumn(
                name: "十六烷指数",
                table: "Assays");

            migrationBuilder.DropColumn(
                name: "含硫",
                table: "Assays");

            migrationBuilder.DropColumn(
                name: "蚀点",
                table: "Assays");
        }
    }
}
