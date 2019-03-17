using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovelMultilanguageSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebNovelChapters_Languages_LanguageId",
                table: "WebNovelChapters");

            migrationBuilder.DropForeignKey(
                name: "FK_WebNovelChapters_AspNetUsers_UploaderId",
                table: "WebNovelChapters");

            migrationBuilder.DropIndex(
                name: "IX_WebNovelChapters_LanguageId",
                table: "WebNovelChapters");

            migrationBuilder.DropIndex(
                name: "IX_WebNovelChapters_UploaderId",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "FreeToAccessDate",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "UploaderId",
                table: "WebNovelChapters");

            migrationBuilder.CreateTable(
                name: "UserLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLanguages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebNovelChapterContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    FreeToAccessDate = table.Column<DateTime>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    UploaderId = table.Column<string>(nullable: false),
                    ChapterId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelChapterContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebNovelChapterContent_WebNovelChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "WebNovelChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebNovelChapterContent_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WebNovelChapterContent_AspNetUsers_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguages_LanguageId",
                table: "UserLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguages_UserId",
                table: "UserLanguages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapterContent_ChapterId",
                table: "WebNovelChapterContent",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapterContent_LanguageId",
                table: "WebNovelChapterContent",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapterContent_UploaderId",
                table: "WebNovelChapterContent",
                column: "UploaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLanguages");

            migrationBuilder.DropTable(
                name: "WebNovelChapterContent");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FreeToAccessDate",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "WebNovelChapters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UploaderId",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapters_LanguageId",
                table: "WebNovelChapters",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapters_UploaderId",
                table: "WebNovelChapters",
                column: "UploaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovelChapters_Languages_LanguageId",
                table: "WebNovelChapters",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebNovelChapters_AspNetUsers_UploaderId",
                table: "WebNovelChapters",
                column: "UploaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
