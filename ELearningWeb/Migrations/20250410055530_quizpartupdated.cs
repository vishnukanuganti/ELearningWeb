using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearningWeb.Migrations
{
    /// <inheritdoc />
    public partial class quizpartupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Classes_ClassId",
                table: "Quizzes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "Quizzes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "QuizAttempts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizId1",
                table: "QuizAttempts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_ApplicationUserId",
                table: "QuizAttempts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId1",
                table: "QuizAttempts",
                column: "QuizId1");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_ApplicationUserId",
                table: "QuizAttempts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId1",
                table: "QuizAttempts",
                column: "QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Classes_ClassId",
                table: "Quizzes",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_ApplicationUserId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId1",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Classes_ClassId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempts_ApplicationUserId",
                table: "QuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempts_QuizId1",
                table: "QuizAttempts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "QuizAttempts");

            migrationBuilder.DropColumn(
                name: "QuizId1",
                table: "QuizAttempts");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quizzes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Classes_ClassId",
                table: "Quizzes",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
