using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class AllTable_IsDel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Wages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Surveys",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "StoreTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Stores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "ProductTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Payments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Notices",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "InAndOutLogs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Companys",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "ChargeLogs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDel",
                table: "Assays",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Wages");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "StoreTypes");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Notices");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "InAndOutLogs");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "ChargeLogs");

            migrationBuilder.DropColumn(
                name: "IsDel",
                table: "Assays");
        }
    }
}
