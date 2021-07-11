﻿// <auto-generated />
using System;
using JacksonVeroneze.StockService.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace JacksonVeroneze.StockService.Infra.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210711132551_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("stock")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("AdjustmentItemMovementItem", b =>
                {
                    b.Property<Guid>("AdjustmentItemsId")
                        .HasColumnType("uuid")
                        .HasColumnName("adjustment_items_id");

                    b.Property<Guid>("MovementItemsId")
                        .HasColumnType("uuid")
                        .HasColumnName("movement_items_id");

                    b.HasKey("AdjustmentItemsId", "MovementItemsId")
                        .HasName("pk_adjustment_item_movement_item");

                    b.HasIndex("MovementItemsId")
                        .HasDatabaseName("ix_adjustment_item_movement_item_movement_items_id");

                    b.ToTable("adjustment_item_movement_item");
                });

            modelBuilder.Entity("DevolutionItemMovementItem", b =>
                {
                    b.Property<Guid>("DevolutionItemsId")
                        .HasColumnType("uuid")
                        .HasColumnName("devolution_items_id");

                    b.Property<Guid>("MovementItemsId")
                        .HasColumnType("uuid")
                        .HasColumnName("movement_items_id");

                    b.HasKey("DevolutionItemsId", "MovementItemsId")
                        .HasName("pk_devolution_item_movement_item");

                    b.HasIndex("MovementItemsId")
                        .HasDatabaseName("ix_devolution_item_movement_item_movement_items_id");

                    b.ToTable("devolution_item_movement_item");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Adjustment", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_adjustment");

                    b.ToTable("adjustment");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.AdjustmentItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AdjustmentId")
                        .HasColumnType("uuid")
                        .HasColumnName("adjustment_id");

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_adjustment_item");

                    b.HasIndex("AdjustmentId")
                        .HasDatabaseName("ix_adjustment_item_adjustment_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_adjustment_item_product_id");

                    b.ToTable("adjustment_item");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Devolution", b =>
                {
                    b.Property<Guid>("Id")
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
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("description");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_devolution");

                    b.ToTable("devolution");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.DevolutionItem", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<Guid>("DevolutionId")
                        .HasColumnType("uuid")
                        .HasColumnName("devolution_id");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_devolution_item");

                    b.HasIndex("DevolutionId")
                        .HasDatabaseName("ix_devolution_item_devolution_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_devolution_item_product_id");

                    b.ToTable("devolution_item");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Movement", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_movement");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_movement_product_id");

                    b.ToTable("movement");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.MovementItem", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<Guid>("MovementId")
                        .HasColumnType("uuid")
                        .HasColumnName("movement_id");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_movement_item");

                    b.HasIndex("MovementId")
                        .HasDatabaseName("ix_movement_item_movement_id");

                    b.ToTable("movement_item");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Output", b =>
                {
                    b.Property<Guid>("Id")
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
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("description");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_output");

                    b.ToTable("output");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.OutputItem", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<Guid>("OutputId")
                        .HasColumnType("uuid")
                        .HasColumnName("output_id");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_output_item");

                    b.HasIndex("OutputId")
                        .HasDatabaseName("ix_output_item_output_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_output_item_product_id");

                    b.ToTable("output_item");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_product");

                    b.HasIndex("Description", "IsActive")
                        .IsUnique()
                        .HasDatabaseName("ix_product_description_is_active");

                    b.ToTable("product");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Purchase", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_purchase");

                    b.ToTable("purchase");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.PurchaseItem", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<Guid>("PurchaseId")
                        .HasColumnType("uuid")
                        .HasColumnName("purchase_id");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<decimal>("Value")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("value");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_purchase_item");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_purchase_item_product_id");

                    b.HasIndex("PurchaseId")
                        .HasDatabaseName("ix_purchase_item_purchase_id");

                    b.ToTable("purchase_item");
                });

            modelBuilder.Entity("MovementItemOutputItem", b =>
                {
                    b.Property<Guid>("MovementItemsId")
                        .HasColumnType("uuid")
                        .HasColumnName("movement_items_id");

                    b.Property<Guid>("OutputItemsId")
                        .HasColumnType("uuid")
                        .HasColumnName("output_items_id");

                    b.HasKey("MovementItemsId", "OutputItemsId")
                        .HasName("pk_movement_item_output_item");

                    b.HasIndex("OutputItemsId")
                        .HasDatabaseName("ix_movement_item_output_item_output_items_id");

                    b.ToTable("movement_item_output_item");
                });

            modelBuilder.Entity("MovementItemPurchaseItem", b =>
                {
                    b.Property<Guid>("MovementItemsId")
                        .HasColumnType("uuid")
                        .HasColumnName("movement_items_id");

                    b.Property<Guid>("PurchaseItemsId")
                        .HasColumnType("uuid")
                        .HasColumnName("purchase_items_id");

                    b.HasKey("MovementItemsId", "PurchaseItemsId")
                        .HasName("pk_movement_item_purchase_item");

                    b.HasIndex("PurchaseItemsId")
                        .HasDatabaseName("ix_movement_item_purchase_item_purchase_items_id");

                    b.ToTable("movement_item_purchase_item");
                });

            modelBuilder.Entity("AdjustmentItemMovementItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.AdjustmentItem", null)
                        .WithMany()
                        .HasForeignKey("AdjustmentItemsId")
                        .HasConstraintName("fk_adjustment_item_movement_item_adjustment_item_adjustment_ite")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.MovementItem", null)
                        .WithMany()
                        .HasForeignKey("MovementItemsId")
                        .HasConstraintName("fk_adjustment_item_movement_item_movement_item_movement_items_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevolutionItemMovementItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.DevolutionItem", null)
                        .WithMany()
                        .HasForeignKey("DevolutionItemsId")
                        .HasConstraintName("fk_devolution_item_movement_item_devolution_item_devolution_ite")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.MovementItem", null)
                        .WithMany()
                        .HasForeignKey("MovementItemsId")
                        .HasConstraintName("fk_devolution_item_movement_item_movement_item_movement_items_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.AdjustmentItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Adjustment", "Adjustment")
                        .WithMany("Items")
                        .HasForeignKey("AdjustmentId")
                        .HasConstraintName("fk_adjustment_item_adjustment_adjustment_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Product", "Product")
                        .WithMany("ItemsAdjustment")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_adjustment_item_product_product_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Adjustment");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.DevolutionItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Devolution", "Devolution")
                        .WithMany("Items")
                        .HasForeignKey("DevolutionId")
                        .HasConstraintName("fk_devolution_item_devolution_devolution_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Product", "Product")
                        .WithMany("ItemsDevolution")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_devolution_item_product_product_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Devolution");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Movement", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Product", "Product")
                        .WithMany("ItemsMovement")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_movement_product_product_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.MovementItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Movement", "Movement")
                        .WithMany("Items")
                        .HasForeignKey("MovementId")
                        .HasConstraintName("fk_movement_item_movement_movement_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movement");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.OutputItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Output", "Output")
                        .WithMany("Items")
                        .HasForeignKey("OutputId")
                        .HasConstraintName("fk_output_item_output_output_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Product", "Product")
                        .WithMany("ItemsOutput")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_output_item_product_product_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Output");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.PurchaseItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Product", "Product")
                        .WithMany("ItemsPurchase")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_purchase_item_product_product_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.Purchase", "Purchase")
                        .WithMany("Items")
                        .HasForeignKey("PurchaseId")
                        .HasConstraintName("fk_purchase_item_purchase_purchase_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("MovementItemOutputItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.MovementItem", null)
                        .WithMany()
                        .HasForeignKey("MovementItemsId")
                        .HasConstraintName("fk_movement_item_output_item_movement_item_movement_items_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.OutputItem", null)
                        .WithMany()
                        .HasForeignKey("OutputItemsId")
                        .HasConstraintName("fk_movement_item_output_item_output_item_output_items_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovementItemPurchaseItem", b =>
                {
                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.MovementItem", null)
                        .WithMany()
                        .HasForeignKey("MovementItemsId")
                        .HasConstraintName("fk_movement_item_purchase_item_movement_item_movement_items_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JacksonVeroneze.StockService.Domain.Entities.PurchaseItem", null)
                        .WithMany()
                        .HasForeignKey("PurchaseItemsId")
                        .HasConstraintName("fk_movement_item_purchase_item_purchase_item_purchase_items_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Adjustment", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Devolution", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Movement", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Output", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Product", b =>
                {
                    b.Navigation("ItemsAdjustment");

                    b.Navigation("ItemsDevolution");

                    b.Navigation("ItemsMovement");

                    b.Navigation("ItemsOutput");

                    b.Navigation("ItemsPurchase");
                });

            modelBuilder.Entity("JacksonVeroneze.StockService.Domain.Entities.Purchase", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
