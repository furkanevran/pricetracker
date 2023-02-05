using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IndexChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserProducts_UserId",
                table: "UserProducts");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "UserProducts",
                type: "TEXT",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TokenVersion",
                table: "Users",
                column: "TokenVersion");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_Tag",
                table: "UserProducts",
                column: "Tag");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_UserId_TrackingProductId",
                table: "UserProducts",
                columns: new[] { "UserId", "TrackingProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingProductPrices_AddedAt",
                table: "TrackingProductPrices",
                column: "AddedAt",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingProductPrices_Price",
                table: "TrackingProductPrices",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingProductPrices_TrackingProductId_AddedAt",
                table: "TrackingProductPrices",
                columns: new[] { "TrackingProductId", "AddedAt" },
                unique: true,
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumedRefreshTokens_ExpiresAt",
                table: "ConsumedRefreshTokens",
                column: "ExpiresAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_TokenVersion",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserProducts_Tag",
                table: "UserProducts");

            migrationBuilder.DropIndex(
                name: "IX_UserProducts_UserId_TrackingProductId",
                table: "UserProducts");

            migrationBuilder.DropIndex(
                name: "IX_TrackingProductPrices_AddedAt",
                table: "TrackingProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_TrackingProductPrices_Price",
                table: "TrackingProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_TrackingProductPrices_TrackingProductId_AddedAt",
                table: "TrackingProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_ConsumedRefreshTokens_ExpiresAt",
                table: "ConsumedRefreshTokens");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "UserProducts",
                type: "TEXT",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 120);

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_UserId",
                table: "UserProducts",
                column: "UserId");
        }
    }
}
