using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MFS.Models;

namespace MFS.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20170720012417_SalesPlan")]
    partial class SalesPlan
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MFS.Models.SalesPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CarNo");

                    b.Property<int>("Count");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<bool>("IsInvoice");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("OilDate");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Remainder");

                    b.Property<int>("SalesPlanType");

                    b.Property<string>("Unit");

                    b.HasKey("Id");

                    b.ToTable("SalesPlans");
                });
        }
    }
}
