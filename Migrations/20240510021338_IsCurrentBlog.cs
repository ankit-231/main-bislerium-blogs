using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bislerium_blogs.Migrations
{
    /// <inheritdoc />
    public partial class IsCurrentBlog : Migration
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

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "BlogModel",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTimestamp",
                table: "BlogModel",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                name: "FK_CommentModel_AspNetUsers_UserId",
                table: "CommentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentModel_BlogModel_BlogId",
                table: "CommentModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionModel_AspNetUsers_UserId",
                table: "ReactionModel");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "BlogModel");

            migrationBuilder.DropColumn(
                name: "UpdatedTimestamp",
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
