using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchAPI.Migrations
{
    public partial class RatingColumnSpellingCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reating",
                table: "Products",
                newName: "Rating");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Products",
                newName: "Reating");
        }
    }
}
