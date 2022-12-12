using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Persistence.Migrations
{
    public partial class TrackingProductAndPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackingProducts",
                columns: table => new
                {
                    TrackingProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    AddedByUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingProducts", x => x.TrackingProductId);
                    table.ForeignKey(
                        name: "FK_TrackingProducts_Users_AddedByUserId",
                        column: x => x.AddedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackingProductPrices",
                columns: table => new
                {
                    TrackingProductPriceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrackingProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingProductPrices", x => x.TrackingProductPriceId);
                    table.ForeignKey(
                        name: "FK_TrackingProductPrices_TrackingProducts_TrackingProductId",
                        column: x => x.TrackingProductId,
                        principalTable: "TrackingProducts",
                        principalColumn: "TrackingProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingProductPrices_TrackingProductId",
                table: "TrackingProductPrices",
                column: "TrackingProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingProducts_AddedByUserId",
                table: "TrackingProducts",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingProducts_Url",
                table: "TrackingProducts",
                column: "Url",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackingProductPrices");

            migrationBuilder.DropTable(
                name: "TrackingProducts");
        }
    }
}
