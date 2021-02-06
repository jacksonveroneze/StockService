﻿// <auto-generated />
using System;
using JacksonVeroneze.StockService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace JacksonVeroneze.StockService.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_product");

                    b.ToTable("product");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Purchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("numeric")
                        .HasColumnName("total_value");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_purchase");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_purchase_product_id");

                    b.ToTable("purchase");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.PurchaseItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_at");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric")
                        .HasColumnName("value");

                    b.Property<int>("Version")
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.Property<Guid>("purchase_id")
                        .HasColumnType("uuid")
                        .HasColumnName("purchase_id");

                    b.HasKey("Id")
                        .HasName("pk_purchase_item");

                    b.HasIndex("purchase_id")
                        .HasDatabaseName("ix_purchase_item_purchase_id");

                    b.ToTable("purchase_item");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Purchase", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Product", null)
                        .WithMany("Items")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_purchase_product_product_id");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.PurchaseItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Purchase", "Purchase")
                        .WithMany("Items")
                        .HasForeignKey("purchase_id")
                        .HasConstraintName("fk_purchase_item_purchase_purchase_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Product", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Purchase", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
