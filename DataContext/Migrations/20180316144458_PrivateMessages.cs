using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DataContext.Migrations
{
    public partial class PrivateMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "NewsComments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PrivateMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    IsSentByAdministration = table.Column<bool>(nullable: false),
                    ReceiverId = table.Column<string>(nullable: false),
                    SenderId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateMessages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivateMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsComments_ParentId",
                table: "NewsComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_ReceiverId",
                table: "PrivateMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_SenderId",
                table: "PrivateMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsComments_NewsComments_ParentId",
                table: "NewsComments",
                column: "ParentId",
                principalTable: "NewsComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsComments_NewsComments_ParentId",
                table: "NewsComments");

            migrationBuilder.DropTable(
                name: "PrivateMessages");

            migrationBuilder.DropIndex(
                name: "IX_NewsComments_ParentId",
                table: "NewsComments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "NewsComments");
        }
    }
}
