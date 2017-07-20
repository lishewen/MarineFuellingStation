using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MFS.Migrations
{
    public partial class SalesPlan_Audit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AuditTime",
                table: "SalesPlans",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Auditor",
                table: "SalesPlans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "SalesPlans",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditTime",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "Auditor",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "State",
                table: "SalesPlans");
        }
    }
}
