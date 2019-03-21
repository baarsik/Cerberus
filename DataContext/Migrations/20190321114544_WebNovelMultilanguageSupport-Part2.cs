using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovelMultilanguageSupportPart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WebNovels");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "WebNovels");

            migrationBuilder.CreateTable(
                name: "WebNovelContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    WebNovelId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebNovelContent_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebNovelContent_WebNovels_WebNovelId",
                        column: x => x.WebNovelId,
                        principalTable: "WebNovels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelContent_LanguageId",
                table: "WebNovelContent",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelContent_WebNovelId",
                table: "WebNovelContent",
                column: "WebNovelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebNovelContent");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WebNovels",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WebNovels",
                nullable: false,
                defaultValue: "");
        }
    }
}
