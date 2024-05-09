using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bislerium_blogs.Migrations
{
    /// <inheritdoc />
    public partial class SelfCommentReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "CommentModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_ParentCommentId",
                table: "CommentModel",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentModel_CommentModel_ParentCommentId",
                table: "CommentModel",
                column: "ParentCommentId",
                principalTable: "CommentModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentModel_CommentModel_ParentCommentId",
                table: "CommentModel");

            migrationBuilder.DropIndex(
                name: "IX_CommentModel_ParentCommentId",
                table: "CommentModel");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "CommentModel");
        }
    }
}
