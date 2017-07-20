using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MFS.Migrations
{
    public partial class SalesPlan_Billing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillingCompany",
                table: "SalesPlans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BillingCount",
                table: "SalesPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "BillingPrice",
                table: "SalesPlans",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingCompany",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "BillingCount",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "BillingPrice",
                table: "SalesPlans");
        }
    }
}
