using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class EntityBase_DelReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Wages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Surveys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "StoreTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Stores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "SalesPlans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "ProductTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Notices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "MoveStores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "InAndOutLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Companys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "ChargeLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "BoatCleans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelReason",
                table: "Assays",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Wages");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "StoreTypes");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "SalesPlans");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "MoveStores");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "InAndOutLogs");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "ChargeLogs");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "BoatCleans");

            migrationBuilder.DropColumn(
                name: "DelReason",
                table: "Assays");
        }
    }
}
