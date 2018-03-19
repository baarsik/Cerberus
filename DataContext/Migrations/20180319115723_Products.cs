using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DataContext.Migrations
{
    public partial class Products : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_ForumThreads_ForumThreadId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_News_NewsId",
                table: "Attachments");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Attachments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(maxLength: 64, nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Value = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSettings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ProductId",
                table: "Attachments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSettings_ProductId",
                table: "ProductSettings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSettings_UserId",
                table: "ProductSettings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_ForumThreads_ForumThreadId",
                table: "Attachments",
                column: "ForumThreadId",
                principalTable: "ForumThreads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_News_NewsId",
                table: "Attachments",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Products_ProductId",
                table: "Attachments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_ForumThreads_ForumThreadId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_News_NewsId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Products_ProductId",
                table: "Attachments");

            migrationBuilder.DropTable(
                name: "ProductSettings");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ProductId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Attachments");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_ForumThreads_ForumThreadId",
                table: "Attachments",
                column: "ForumThreadId",
                principalTable: "ForumThreads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_News_NewsId",
                table: "Attachments",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
