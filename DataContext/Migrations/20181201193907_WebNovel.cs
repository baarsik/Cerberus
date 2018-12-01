using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Forums",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebNovelChapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    FreeToAccessDate = table.Column<DateTime>(nullable: false),
                    NextChapterId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelChapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebNovelChapters_WebNovelChapters_NextChapterId",
                        column: x => x.NextChapterId,
                        principalTable: "WebNovelChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WebNovelChapterAccess",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    ChapterId = table.Column<Guid>(nullable: false),
                    AccessPurchaseDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelChapterAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebNovelChapterAccess_WebNovelChapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "WebNovelChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovelChapterAccess_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebNovels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UrlName = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CoverUrl = table.Column<string>(nullable: true),
                    OriginalName = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    FirstChapterId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebNovels_WebNovelChapters_FirstChapterId",
                        column: x => x.FirstChapterId,
                        principalTable: "WebNovelChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapterAccess_ChapterId",
                table: "WebNovelChapterAccess",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapterAccess_UserId",
                table: "WebNovelChapterAccess",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelChapters_NextChapterId",
                table: "WebNovelChapters",
                column: "NextChapterId",
                unique: true,
                filter: "[NextChapterId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovels_FirstChapterId",
                table: "WebNovels",
                column: "FirstChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovels_UrlName",
                table: "WebNovels",
                column: "UrlName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "WebNovelChapterAccess");

            migrationBuilder.DropTable(
                name: "WebNovels");

            migrationBuilder.DropTable(
                name: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Forums");
        }
    }
}
