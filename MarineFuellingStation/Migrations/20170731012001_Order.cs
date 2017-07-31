using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MFS.Migrations
{
    public partial class Order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillingCompany = table.Column<string>(nullable: true),
                    BillingCount = table.Column<int>(nullable: false),
                    BillingPrice = table.Column<decimal>(nullable: false),
                    CarNo = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Density = table.Column<decimal>(nullable: false),
                    DiffOil = table.Column<decimal>(nullable: false),
                    DiffWeight = table.Column<decimal>(nullable: false),
                    EmptyCarWeight = table.Column<decimal>(nullable: false),
                    EndOilDateTime = table.Column<DateTime>(nullable: true),
                    Instrument1 = table.Column<decimal>(nullable: false),
                    Instrument2 = table.Column<decimal>(nullable: false),
                    Instrument3 = table.Column<decimal>(nullable: false),
                    IsInvoice = table.Column<bool>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    OilCarWeight = table.Column<decimal>(nullable: false),
                    OilCount = table.Column<decimal>(nullable: false),
                    OilTemperature = table.Column<decimal>(nullable: false),
                    OrderType = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    SalesCommission = table.Column<decimal>(nullable: false),
                    SalesPlanId = table.Column<int>(nullable: true),
                    StartOilDateTime = table.Column<DateTime>(nullable: true),
                    TransportOrderId = table.Column<int>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Worker = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_SalesPlans_SalesPlanId",
                        column: x => x.SalesPlanId,
                        principalTable: "SalesPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SalesPlanId",
                table: "Orders",
                column: "SalesPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
