using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class purchase_ToStoreinstruAf_ToStoreInstruBf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToStoreInstruAf",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToStoreInstruBf",
                table: "Purchases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToStoreInstruAf",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ToStoreInstruBf",
                table: "Purchases");
        }
    }
}
