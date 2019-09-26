using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovelReaderData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebNovelReaderData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NotificationsEnabled = table.Column<bool>(nullable: false),
                    Rating = table.Column<int>(nullable: true),
                    WebNovelId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelReaderData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebNovelReaderData_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovelReaderData_WebNovels_WebNovelId",
                        column: x => x.WebNovelId,
                        principalTable: "WebNovels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelReaderData_UserId",
                table: "WebNovelReaderData",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelReaderData_WebNovelId",
                table: "WebNovelReaderData",
                column: "WebNovelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebNovelReaderData");
        }
    }
}
