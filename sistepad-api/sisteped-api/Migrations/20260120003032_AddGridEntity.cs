using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chronovault_api.Migrations
{
    /// <inheritdoc />
    public partial class AddGridEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GridId",
                table: "Grades",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Grids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grids", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GridId",
                table: "Grades",
                column: "GridId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Grids_GridId",
                table: "Grades",
                column: "GridId",
                principalTable: "Grids",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Grids_GridId",
                table: "Grades");

            migrationBuilder.DropTable(
                name: "Grids");

            migrationBuilder.DropIndex(
                name: "IX_Grades_GridId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "GridId",
                table: "Grades");
        }
    }
}
