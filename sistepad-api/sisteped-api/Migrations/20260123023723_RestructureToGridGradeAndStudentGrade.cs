using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chronovault_api.Migrations
{
    /// <inheritdoc />
    public partial class RestructureToGridGradeAndStudentGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Grades_GradeId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Grades_GradeId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Grids_GridId",
                table: "Grades");

            migrationBuilder.DropTable(
                name: "GradeClasses");

            migrationBuilder.DropIndex(
                name: "IX_Grades_GridId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "GridId",
                table: "Grades");

            migrationBuilder.RenameColumn(
                name: "GradeId",
                table: "Attendances",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_GradeId",
                table: "Attendances",
                newName: "IX_Attendances_ClassId");

            migrationBuilder.RenameColumn(
                name: "GradeId",
                table: "Activities",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_GradeId",
                table: "Activities",
                newName: "IX_Activities_ClassId");

            migrationBuilder.CreateTable(
                name: "GridClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GridId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GridClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GridClasses_Grids_GridId",
                        column: x => x.GridId,
                        principalTable: "Grids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GridGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GridId = table.Column<int>(type: "INTEGER", nullable: false),
                    GradeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GridGrades_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GridGrades_Grids_GridId",
                        column: x => x.GridId,
                        principalTable: "Grids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GridClasses_ClassId",
                table: "GridClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GridClasses_GridId",
                table: "GridClasses",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_GridGrades_GradeId",
                table: "GridGrades",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GridGrades_GridId",
                table: "GridGrades",
                column: "GridId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Classes_ClassId",
                table: "Activities",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Classes_ClassId",
                table: "Attendances",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Classes_ClassId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Classes_ClassId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "GridClasses");

            migrationBuilder.DropTable(
                name: "GridGrades");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Attendances",
                newName: "GradeId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances",
                newName: "IX_Attendances_GradeId");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Activities",
                newName: "GradeId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ClassId",
                table: "Activities",
                newName: "IX_Activities_GradeId");

            migrationBuilder.AddColumn<int>(
                name: "GridId",
                table: "Grades",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GradeClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    GradeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeClasses_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GridId",
                table: "Grades",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeClasses_ClassId",
                table: "GradeClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeClasses_GradeId",
                table: "GradeClasses",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Grades_GradeId",
                table: "Activities",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Grades_GradeId",
                table: "Attendances",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Grids_GridId",
                table: "Grades",
                column: "GridId",
                principalTable: "Grids",
                principalColumn: "Id");
        }
    }
}
