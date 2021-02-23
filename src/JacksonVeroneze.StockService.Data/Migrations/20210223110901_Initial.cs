using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JacksonVeroneze.StockService.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adjustment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adjustment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "output",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_output", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "purchase",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchase", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "adjustment_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    adjustment_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adjustment_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_adjustment_item_adjustment_adjustment_id",
                        column: x => x.adjustment_id,
                        principalTable: "adjustment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_adjustment_item_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movement", x => x.id);
                    table.ForeignKey(
                        name: "fk_movement_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "output_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    output_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_output_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_output_item_output_output_id",
                        column: x => x.output_id,
                        principalTable: "output",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_output_item_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    purchase_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchase_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_purchase_item_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_purchase_item_purchase_purchase_id",
                        column: x => x.purchase_id,
                        principalTable: "purchase",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movement_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    movement_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movement_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_movement_item_movement_movement_id",
                        column: x => x.movement_id,
                        principalTable: "movement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_adjustment_item_adjustment_id",
                table: "adjustment_item",
                column: "adjustment_id");

            migrationBuilder.CreateIndex(
                name: "ix_adjustment_item_product_id",
                table: "adjustment_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_movement_product_id",
                table: "movement",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_movement_item_movement_id",
                table: "movement_item",
                column: "movement_id");

            migrationBuilder.CreateIndex(
                name: "ix_output_item_output_id",
                table: "output_item",
                column: "output_id");

            migrationBuilder.CreateIndex(
                name: "ix_output_item_product_id",
                table: "output_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_description_is_active",
                table: "product",
                columns: new[] { "description", "is_active" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_purchase_item_product_id",
                table: "purchase_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_purchase_item_purchase_id",
                table: "purchase_item",
                column: "purchase_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adjustment_item");

            migrationBuilder.DropTable(
                name: "movement_item");

            migrationBuilder.DropTable(
                name: "output_item");

            migrationBuilder.DropTable(
                name: "purchase_item");

            migrationBuilder.DropTable(
                name: "adjustment");

            migrationBuilder.DropTable(
                name: "movement");

            migrationBuilder.DropTable(
                name: "output");

            migrationBuilder.DropTable(
                name: "purchase");

            migrationBuilder.DropTable(
                name: "product");
        }
    }
}
