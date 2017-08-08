using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MFS.Migrations
{
    public partial class StoreEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvgPrice = table.Column<decimal>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    IsUse = table.Column<bool>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    LastValue = table.Column<decimal>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    StoreClass = table.Column<int>(nullable: false),
                    StoreTypeId = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_StoreTypes_StoreTypeId",
                        column: x => x.StoreTypeId,
                        principalTable: "StoreTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreTypeId",
                table: "Stores",
                column: "StoreTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "StoreTypes");
        }
    }
}
