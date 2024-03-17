using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchAPI.Migrations
{
    public partial class SearchHistoryTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SearchCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SearchStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultCount = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchHistories");
        }
    }
}
