﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFinalProj.Models;

namespace MyFinalProj.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20220702113623_StoreMigration")]
    partial class StoreMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyFinalProj.Models.Basket", b =>
                {
                    b.Property<int>("BasketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AmountOfItem")
                        .HasColumnType("int");

                    b.Property<int>("AmountToPay")
                        .HasColumnType("int");

                    b.HasKey("BasketId");

                    b.ToTable("Basket");
                });

            modelBuilder.Entity("MyFinalProj.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MyFinalProj.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AmountAvailable")
                        .HasColumnType("int");

                    b.Property<int?>("AmountToBuy")
                        .HasColumnType("int");

                    b.Property<int?>("BasketId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PicturePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PurchaseId")
                        .HasColumnType("int");

                    b.Property<int?>("SellerId")
                        .HasColumnType("int");

                    b.HasKey("ItemId");

                    b.HasIndex("BasketId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PurchaseId");

                    b.HasIndex("SellerId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("MyFinalProj.Models.Purchase", b =>
                {
                    b.Property<int>("PurchaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("PurchaseEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PurchaseStart")
                        .HasColumnType("datetime2");

                    b.HasKey("PurchaseId");

                    b.ToTable("Purchase");
                });

            modelBuilder.Entity("MyFinalProj.Models.Seller", b =>
                {
                    b.Property<int>("SellerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SellerId");

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("MyFinalProj.Models.Item", b =>
                {
                    b.HasOne("MyFinalProj.Models.Basket", "Basket")
                        .WithMany("Items")
                        .HasForeignKey("BasketId");

                    b.HasOne("MyFinalProj.Models.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId");

                    b.HasOne("MyFinalProj.Models.Purchase", "Purchase")
                        .WithMany()
                        .HasForeignKey("PurchaseId");

                    b.HasOne("MyFinalProj.Models.Seller", "Seller")
                        .WithMany("Items")
                        .HasForeignKey("SellerId");

                    b.Navigation("Basket");

                    b.Navigation("Category");

                    b.Navigation("Purchase");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("MyFinalProj.Models.Basket", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("MyFinalProj.Models.Category", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("MyFinalProj.Models.Seller", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}