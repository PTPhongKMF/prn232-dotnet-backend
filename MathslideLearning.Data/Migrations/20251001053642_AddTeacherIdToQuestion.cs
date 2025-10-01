using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathslideLearning.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherIdToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TeacherId",
                table: "Questions",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_TeacherId",
                table: "Questions",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_TeacherId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TeacherId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Questions");
        }
    }
}
