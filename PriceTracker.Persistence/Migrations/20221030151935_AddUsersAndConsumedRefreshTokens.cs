using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceTracker.Persistence.Migrations
{
    public partial class AddUsersAndConsumedRefreshTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumedRefreshTokens",
                columns: table => new
                {
                    ConsumedRefreshTokenId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumedRefreshTokens", x => x.ConsumedRefreshTokenId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EMail = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordSalt = table.Column<string>(type: "TEXT", nullable: false),
                    TokenVersion = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumedRefreshTokens_ConsumedRefreshTokenId",
                table: "ConsumedRefreshTokens",
                column: "ConsumedRefreshTokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EMail",
                table: "Users",
                column: "EMail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumedRefreshTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
