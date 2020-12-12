using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class WebNovelSymbols_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Symbols",
                table: "WebNovelContent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Symbols",
                table: "WebNovelChapterContent",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbols",
                table: "WebNovelContent");

            migrationBuilder.DropColumn(
                name: "Symbols",
                table: "WebNovelChapterContent");
        }
    }
}
