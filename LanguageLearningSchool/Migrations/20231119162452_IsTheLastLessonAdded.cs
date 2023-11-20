using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageLearningSchool.Migrations
{
    /// <inheritdoc />
    public partial class IsTheLastLessonAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTheLastLesson",
                table: "Courses");

            migrationBuilder.AddColumn<bool>(
                name: "IsTheLastLesson",
                table: "Lessons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTheLastLesson",
                table: "Lessons");

            migrationBuilder.AddColumn<bool>(
                name: "IsTheLastLesson",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
