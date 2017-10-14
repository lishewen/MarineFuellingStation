using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class purchase_auditor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AuditTime",
                table: "Purchases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Auditor",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditTime",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Auditor",
                table: "Purchases");
        }
    }
}
