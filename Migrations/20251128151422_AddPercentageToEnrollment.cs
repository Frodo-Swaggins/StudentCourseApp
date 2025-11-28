using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentCourseApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPercentageToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "Enrollments",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Enrollments");
        }
    }
}
