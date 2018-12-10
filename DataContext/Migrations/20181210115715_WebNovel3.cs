using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNovelChapters_WebNovels_WebNovelId",
                table: "WebNovelChapters");

            migrationBuilder.AddColumn<bool>(
                name: "UsesVolumes",
                table: "WebNovels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "WebNovelId",
                table: "WebNovelChapters",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploaderId",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Volume",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapters_UploaderId",
                table: "WebNovelChapters",
                column: "UploaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovelChapters_AspNetUsers_UploaderId",
                table: "WebNovelChapters",
                column: "UploaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovelChapters_WebNovels_WebNovelId",
                table: "WebNovelChapters",
                column: "WebNovelId",
                principalTable: "WebNovels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNovelChapters_AspNetUsers_UploaderId",
                table: "WebNovelChapters");

            migrationBuilder.DropForeignKey(
                name: "FK_WebNovelChapters_WebNovels_WebNovelId",
                table: "WebNovelChapters");

            migrationBuilder.DropIndex(
                name: "IX_WebNovelChapters_UploaderId",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "UsesVolumes",
                table: "WebNovels");

            migrationBuilder.DropColumn(
                name: "UploaderId",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "WebNovelChapters");

            migrationBuilder.AlterColumn<Guid>(
                name: "WebNovelId",
                table: "WebNovelChapters",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovelChapters_WebNovels_WebNovelId",
                table: "WebNovelChapters",
                column: "WebNovelId",
                principalTable: "WebNovels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
