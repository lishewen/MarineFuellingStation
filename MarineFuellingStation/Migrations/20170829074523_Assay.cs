using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class Assay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssayType = table.Column<int>(type: "int", nullable: false),
                    Assayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUse = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percentage10 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Percentage50 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Percentage90 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: true),
                    SmellType = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    Temperature = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    初硫 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    十六烷值 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    回流 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    干点 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    标密 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    混水反应 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    视密 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    闭口闪点 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assays_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assays_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assays_PurchaseId",
                table: "Assays",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Assays_StoreId",
                table: "Assays",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assays");
        }
    }
}
