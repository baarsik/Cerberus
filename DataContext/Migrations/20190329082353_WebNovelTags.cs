using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovelTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdultContent",
                table: "WebNovels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdultContent",
                table: "WebNovelChapters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdultContentConsentGiven",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WebNovelTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FallbackName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebNovelTagBinding",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebNovelId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelTagBinding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebNovelTagBinding_WebNovelTag_TagId",
                        column: x => x.TagId,
                        principalTable: "WebNovelTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovelTagBinding_WebNovels_WebNovelId",
                        column: x => x.WebNovelId,
                        principalTable: "WebNovels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebNovelTagTranslation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebNovelTagTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebNovelTagTranslation_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebNovelTagTranslation_WebNovelTag_TagId",
                        column: x => x.TagId,
                        principalTable: "WebNovelTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelTagBinding_TagId",
                table: "WebNovelTagBinding",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelTagBinding_WebNovelId",
                table: "WebNovelTagBinding",
                column: "WebNovelId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelTagTranslation_LanguageId",
                table: "WebNovelTagTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_WebNovelTagTranslation_TagId",
                table: "WebNovelTagTranslation",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebNovelTagBinding");

            migrationBuilder.DropTable(
                name: "WebNovelTagTranslation");

            migrationBuilder.DropTable(
                name: "WebNovelTag");

            migrationBuilder.DropColumn(
                name: "IsAdultContent",
                table: "WebNovels");

            migrationBuilder.DropColumn(
                name: "IsAdultContent",
                table: "WebNovelChapters");

            migrationBuilder.DropColumn(
                name: "IsAdultContentConsentGiven",
                table: "AspNetUsers");
        }
    }
}
