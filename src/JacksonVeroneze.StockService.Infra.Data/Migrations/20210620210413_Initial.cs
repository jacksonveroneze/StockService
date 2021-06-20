using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JacksonVeroneze.StockService.Infra.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "stock");

            migrationBuilder.CreateTable(
                name: "adjustment",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adjustment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "output",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_output", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "purchase",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchase", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "adjustment_item",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    adjustment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adjustment_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_adjustment_item_adjustment_adjustment_id",
                        column: x => x.adjustment_id,
                        principalSchema: "stock",
                        principalTable: "adjustment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_adjustment_item_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "stock",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movement",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movement", x => x.id);
                    table.ForeignKey(
                        name: "fk_movement_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "stock",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "output_item",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    output_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_output_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_output_item_output_output_id",
                        column: x => x.output_id,
                        principalSchema: "stock",
                        principalTable: "output",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_output_item_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "stock",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_item",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    purchase_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchase_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_purchase_item_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "stock",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_purchase_item_purchase_purchase_id",
                        column: x => x.purchase_id,
                        principalSchema: "stock",
                        principalTable: "purchase",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movement_item",
                schema: "stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    movement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movement_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_movement_item_movement_movement_id",
                        column: x => x.movement_id,
                        principalSchema: "stock",
                        principalTable: "movement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "adjustment_item_movement_item",
                schema: "stock",
                columns: table => new
                {
                    adjustment_items_id = table.Column<Guid>(type: "uuid", nullable: false),
                    movement_items_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adjustment_item_movement_item", x => new { x.adjustment_items_id, x.movement_items_id });
                    table.ForeignKey(
                        name: "fk_adjustment_item_movement_item_adjustment_item_adjustment_ite",
                        column: x => x.adjustment_items_id,
                        principalSchema: "stock",
                        principalTable: "adjustment_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_adjustment_item_movement_item_movement_item_movement_items_id",
                        column: x => x.movement_items_id,
                        principalSchema: "stock",
                        principalTable: "movement_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movement_item_output_item",
                schema: "stock",
                columns: table => new
                {
                    movement_items_id = table.Column<Guid>(type: "uuid", nullable: false),
                    output_items_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movement_item_output_item", x => new { x.movement_items_id, x.output_items_id });
                    table.ForeignKey(
                        name: "fk_movement_item_output_item_movement_item_movement_items_id",
                        column: x => x.movement_items_id,
                        principalSchema: "stock",
                        principalTable: "movement_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movement_item_output_item_output_item_output_items_id",
                        column: x => x.output_items_id,
                        principalSchema: "stock",
                        principalTable: "output_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movement_item_purchase_item",
                schema: "stock",
                columns: table => new
                {
                    movement_items_id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_items_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movement_item_purchase_item", x => new { x.movement_items_id, x.purchase_items_id });
                    table.ForeignKey(
                        name: "fk_movement_item_purchase_item_movement_item_movement_items_id",
                        column: x => x.movement_items_id,
                        principalSchema: "stock",
                        principalTable: "movement_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movement_item_purchase_item_purchase_item_purchase_items_id",
                        column: x => x.purchase_items_id,
                        principalSchema: "stock",
                        principalTable: "purchase_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_adjustment_item_adjustment_id",
                schema: "stock",
                table: "adjustment_item",
                column: "adjustment_id");

            migrationBuilder.CreateIndex(
                name: "ix_adjustment_item_product_id",
                schema: "stock",
                table: "adjustment_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_adjustment_item_movement_item_movement_items_id",
                schema: "stock",
                table: "adjustment_item_movement_item",
                column: "movement_items_id");

            migrationBuilder.CreateIndex(
                name: "ix_movement_product_id",
                schema: "stock",
                table: "movement",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_movement_item_movement_id",
                schema: "stock",
                table: "movement_item",
                column: "movement_id");

            migrationBuilder.CreateIndex(
                name: "ix_movement_item_output_item_output_items_id",
                schema: "stock",
                table: "movement_item_output_item",
                column: "output_items_id");

            migrationBuilder.CreateIndex(
                name: "ix_movement_item_purchase_item_purchase_items_id",
                schema: "stock",
                table: "movement_item_purchase_item",
                column: "purchase_items_id");

            migrationBuilder.CreateIndex(
                name: "ix_output_item_output_id",
                schema: "stock",
                table: "output_item",
                column: "output_id");

            migrationBuilder.CreateIndex(
                name: "ix_output_item_product_id",
                schema: "stock",
                table: "output_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_description_is_active",
                schema: "stock",
                table: "product",
                columns: new[] { "description", "is_active" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_purchase_item_product_id",
                schema: "stock",
                table: "purchase_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_purchase_item_purchase_id",
                schema: "stock",
                table: "purchase_item",
                column: "purchase_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "adjustment_item_movement_item",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "movement_item_output_item",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "movement_item_purchase_item",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "adjustment_item",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "output_item",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "movement_item",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "purchase_item",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "adjustment",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "output",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "movement",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "purchase",
                schema: "stock");

            migrationBuilder.DropTable(
                name: "product",
                schema: "stock");
        }
    }
}
