using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNovels_WebNovelChapters_FirstChapterId",
                table: "WebNovels");

            migrationBuilder.DropIndex(
                name: "IX_WebNovels_FirstChapterId",
                table: "WebNovels");

            migrationBuilder.DropColumn(
                name: "FirstChapterId",
                table: "WebNovels");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "WebNovels",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WebNovelId",
                table: "WebNovelChapters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapters_WebNovelId",
                table: "WebNovelChapters",
                column: "WebNovelId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovelChapters_WebNovels_WebNovelId",
                table: "WebNovelChapters",
                column: "WebNovelId",
                principalTable: "WebNovels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNovelChapters_WebNovels_WebNovelId",
                table: "WebNovelChapters");

            migrationBuilder.DropIndex(
                name: "IX_WebNovelChapters_WebNovelId",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "WebNovels");

            migrationBuilder.DropColumn(
                name: "WebNovelId",
                table: "WebNovelChapters");

            migrationBuilder.AddColumn<Guid>(
                name: "FirstChapterId",
                table: "WebNovels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebNovels_FirstChapterId",
                table: "WebNovels",
                column: "FirstChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovels_WebNovelChapters_FirstChapterId",
                table: "WebNovels",
                column: "FirstChapterId",
                principalTable: "WebNovelChapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
