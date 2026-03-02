using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IpGeoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ip = table.Column<string>(type: "text", nullable: false),
                    Geolocation = table.Column<string>(type: "text", nullable: false),
                    IpInfoJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Histories_CreatedAt_Ip",
                table: "Histories",
                columns: new[] { "CreatedAt", "Ip" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");
        }
    }
}
