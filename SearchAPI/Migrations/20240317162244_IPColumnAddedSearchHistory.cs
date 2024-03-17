using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchAPI.Migrations
{
    public partial class IPColumnAddedSearchHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestedIP",
                table: "SearchHistories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestedIP",
                table: "SearchHistories");
        }
    }
}
