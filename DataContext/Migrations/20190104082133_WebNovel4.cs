using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovel4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "WebNovels",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "WebNovels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    GlobalName = table.Column<string>(nullable: false),
                    LocalName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    GlobalName = table.Column<string>(nullable: false),
                    LocalName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebNovels_CountryId",
                table: "WebNovels",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapters_LanguageId",
                table: "WebNovelChapters",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovelChapters_Languages_LanguageId",
                table: "WebNovelChapters",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovels_Countries_CountryId",
                table: "WebNovels",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNovelChapters_Languages_LanguageId",
                table: "WebNovelChapters");

            migrationBuilder.DropForeignKey(
                name: "FK_WebNovels_Countries_CountryId",
                table: "WebNovels");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_WebNovels_CountryId",
                table: "WebNovels");

            migrationBuilder.DropIndex(
                name: "IX_WebNovelChapters_LanguageId",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "WebNovels");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "WebNovels");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "WebNovelChapters");
        }
    }
}
