using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class MoveStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoveStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Elapsed = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InDensity = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InFact = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InPlan = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InStoreId = table.Column<int>(type: "int", nullable: false),
                    InStoreTypeId = table.Column<int>(type: "int", nullable: false),
                    InTemperature = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutDensity = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    OutFact = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    OutPlan = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    OutStoreId = table.Column<int>(type: "int", nullable: false),
                    OutStoreTypeId = table.Column<int>(type: "int", nullable: false),
                    OutTemperature = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveStores", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoveStores");
        }
    }
}
