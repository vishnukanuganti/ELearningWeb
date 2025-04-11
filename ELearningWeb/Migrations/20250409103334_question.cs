using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearningWeb.Migrations
{
    /// <inheritdoc />
    public partial class question : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourseProgress_AspNetUsers_StudentId",
                table: "StudentCourseProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourseProgress_Courses_CourseId",
                table: "StudentCourseProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentCourseProgress",
                table: "StudentCourseProgress");

            migrationBuilder.RenameTable(
                name: "StudentCourseProgress",
                newName: "CourseProgress");

            migrationBuilder.RenameIndex(
                name: "IX_StudentCourseProgress_CourseId",
                table: "CourseProgress",
                newName: "IX_CourseProgress_CourseId");

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "QuizAttempts",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answers",
                table: "QuizAttempts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "QuizAttempts",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseProgress",
                table: "CourseProgress",
                columns: new[] { "StudentId", "CourseId" });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Options = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_ClassId",
                table: "QuizAttempts",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseProgress_AspNetUsers_StudentId",
                table: "CourseProgress",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseProgress_Courses_CourseId",
                table: "CourseProgress",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Classes_ClassId",
                table: "QuizAttempts",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseProgress_AspNetUsers_StudentId",
                table: "CourseProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseProgress_Courses_CourseId",
                table: "CourseProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Classes_ClassId",
                table: "QuizAttempts");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempts_ClassId",
                table: "QuizAttempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseProgress",
                table: "CourseProgress");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "QuizAttempts");

            migrationBuilder.RenameTable(
                name: "CourseProgress",
                newName: "StudentCourseProgress");

            migrationBuilder.RenameIndex(
                name: "IX_CourseProgress_CourseId",
                table: "StudentCourseProgress",
                newName: "IX_StudentCourseProgress_CourseId");

            migrationBuilder.AlterColumn<double>(
                name: "Grade",
                table: "QuizAttempts",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answers",
                table: "QuizAttempts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentCourseProgress",
                table: "StudentCourseProgress",
                columns: new[] { "StudentId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourseProgress_AspNetUsers_StudentId",
                table: "StudentCourseProgress",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourseProgress_Courses_CourseId",
                table: "StudentCourseProgress",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
