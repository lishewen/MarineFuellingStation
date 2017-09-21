using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MFS.Migrations
{
    public partial class DBSet_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseWage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IDCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLeave = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDutyTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Security = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SocialSecurity = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    交通 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    借支 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    出勤天数 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    基本 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    安全保障金 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    实发 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    年月 = table.Column<int>(type: "int", nullable: false),
                    应付 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    提成 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    现金 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    社保 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    职务 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    请假 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    超额 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    转卡金额 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    餐费 = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Wages");
        }
    }
}
