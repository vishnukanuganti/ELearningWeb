using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearningWeb.Migrations
{
    /// <inheritdoc />
    public partial class discussionforum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionPosts_Classes_ClassId",
                table: "DiscussionPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionPosts_Courses_CourseId",
                table: "DiscussionPosts");

            migrationBuilder.RenameColumn(
                name: "PostedAt",
                table: "DiscussionReplies",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "PostedAt",
                table: "DiscussionPosts",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "DiscussionReplies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "DiscussionPosts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "DiscussionPosts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "DiscussionPosts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "DiscussionPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionPosts_Classes_ClassId",
                table: "DiscussionPosts",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionPosts_Courses_CourseId",
                table: "DiscussionPosts",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionPosts_Classes_ClassId",
                table: "DiscussionPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionPosts_Courses_CourseId",
                table: "DiscussionPosts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "DiscussionPosts");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "DiscussionReplies",
                newName: "PostedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "DiscussionPosts",
                newName: "PostedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "DiscussionReplies",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "DiscussionPosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "DiscussionPosts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "DiscussionPosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionPosts_Classes_ClassId",
                table: "DiscussionPosts",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionPosts_Courses_CourseId",
                table: "DiscussionPosts",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
