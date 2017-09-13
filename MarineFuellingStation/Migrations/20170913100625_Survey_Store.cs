using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class Survey_Store : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Surveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_StoreId",
                table: "Surveys",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Stores_StoreId",
                table: "Surveys",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Stores_StoreId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_StoreId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Surveys");
        }
    }
}
