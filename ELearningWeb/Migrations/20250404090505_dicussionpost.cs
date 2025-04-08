using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearningWeb.Migrations
{
    /// <inheritdoc />
    public partial class dicussionpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "DiscussionPosts");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "DiscussionPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Classes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionPosts_ClassId",
                table: "DiscussionPosts",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionPosts_Classes_ClassId",
                table: "DiscussionPosts",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionPosts_Classes_ClassId",
                table: "DiscussionPosts");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionPosts_ClassId",
                table: "DiscussionPosts");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "DiscussionPosts");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "DiscussionPosts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Classes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
