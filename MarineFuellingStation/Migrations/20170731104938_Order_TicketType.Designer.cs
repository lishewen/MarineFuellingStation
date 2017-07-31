using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MFS.Models;

namespace MFS.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20170731104938_Order_TicketType")]
    partial class Order_TicketType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MFS.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<decimal>("Balances");

                    b.Property<int>("ClientType");

                    b.Property<int?>("CompanyId");

                    b.Property<string>("Contact");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<int?>("DefaultProductId");

                    b.Property<string>("FollowSalesman");

                    b.Property<string>("IdCard");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<decimal>("MaxOnAccount");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<int>("PlaceType");

                    b.Property<decimal>("TotalAmount");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("DefaultProductId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("MFS.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<decimal>("Balances");

                    b.Property<string>("Bank");

                    b.Property<string>("BusinessAccount");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("InvoiceTitle");

                    b.Property<string>("Keyword");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<string>("TaxFileNumber");

                    b.Property<int>("TicketType");

                    b.Property<decimal>("TotalAmount");

                    b.HasKey("Id");

                    b.ToTable("Companys");
                });

            modelBuilder.Entity("MFS.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BillingCompany");

                    b.Property<int>("BillingCount");

                    b.Property<decimal>("BillingPrice");

                    b.Property<string>("CarNo");

                    b.Property<int>("Count");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<decimal>("Density");

                    b.Property<decimal>("DiffOil");

                    b.Property<decimal>("DiffWeight");

                    b.Property<decimal>("EmptyCarWeight");

                    b.Property<DateTime?>("EndOilDateTime");

                    b.Property<decimal>("Instrument1");

                    b.Property<decimal>("Instrument2");

                    b.Property<decimal>("Instrument3");

                    b.Property<bool>("IsInvoice");

                    b.Property<bool>("IsTrans");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("OilCarWeight");

                    b.Property<decimal>("OilCount");

                    b.Property<decimal>("OilTemperature");

                    b.Property<int>("OrderType");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("SalesCommission");

                    b.Property<int?>("SalesPlanId");

                    b.Property<DateTime?>("StartOilDateTime");

                    b.Property<int>("State");

                    b.Property<int>("TicketType");

                    b.Property<int?>("TransportOrderId");

                    b.Property<string>("Unit");

                    b.Property<string>("Worker");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SalesPlanId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MFS.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<bool>("IsUse");

                    b.Property<decimal>("LastPrice");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<decimal>("MinPrice");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductTypeId");

                    b.HasKey("Id");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MFS.Models.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("MFS.Models.SalesPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("AuditTime");

                    b.Property<string>("Auditor");

                    b.Property<string>("BillingCompany");

                    b.Property<int>("BillingCount");

                    b.Property<decimal>("BillingPrice");

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

                    b.Property<string>("OilName");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductId");

                    b.Property<decimal>("Remainder");

                    b.Property<int>("SalesPlanType");

                    b.Property<int>("State");

                    b.Property<string>("Unit");

                    b.HasKey("Id");

                    b.ToTable("SalesPlans");
                });

            modelBuilder.Entity("MFS.Models.Client", b =>
                {
                    b.HasOne("MFS.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("MFS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("DefaultProductId");
                });

            modelBuilder.Entity("MFS.Models.Order", b =>
                {
                    b.HasOne("MFS.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MFS.Models.SalesPlan", "SalesPlan")
                        .WithMany()
                        .HasForeignKey("SalesPlanId");
                });

            modelBuilder.Entity("MFS.Models.Product", b =>
                {
                    b.HasOne("MFS.Models.ProductType", "ProductType")
                        .WithMany("Products")
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
