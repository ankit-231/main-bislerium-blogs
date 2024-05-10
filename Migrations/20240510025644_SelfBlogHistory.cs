using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bislerium_blogs.Migrations
{
    /// <inheritdoc />
    public partial class SelfBlogHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentModel_AspNetUsers_UserId",
                table: "CommentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentModel_BlogModel_BlogId",
                table: "CommentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionModel_AspNetUsers_UserId",
                table: "ReactionModel");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReactionModel",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentModel",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "CommentModel",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ParentBlogId",
                table: "BlogModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogModel_ParentBlogId",
                table: "BlogModel",
                column: "ParentBlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogModel_BlogModel_ParentBlogId",
                table: "BlogModel",
                column: "ParentBlogId",
                principalTable: "BlogModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentModel_AspNetUsers_UserId",
                table: "CommentModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentModel_BlogModel_BlogId",
                table: "CommentModel",
                column: "BlogId",
                principalTable: "BlogModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionModel_AspNetUsers_UserId",
                table: "ReactionModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogModel_BlogModel_ParentBlogId",
                table: "BlogModel");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentModel_AspNetUsers_UserId",
                table: "CommentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentModel_BlogModel_BlogId",
                table: "CommentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionModel_AspNetUsers_UserId",
                table: "ReactionModel");

            migrationBuilder.DropIndex(
                name: "IX_BlogModel_ParentBlogId",
                table: "BlogModel");

            migrationBuilder.DropColumn(
                name: "ParentBlogId",
                table: "BlogModel");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReactionModel",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentModel",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "CommentModel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentModel_AspNetUsers_UserId",
                table: "CommentModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentModel_BlogModel_BlogId",
                table: "CommentModel",
                column: "BlogId",
                principalTable: "BlogModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionModel_AspNetUsers_UserId",
                table: "ReactionModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
