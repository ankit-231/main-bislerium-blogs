using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bislerium_blogs.Migrations
{
    /// <inheritdoc />
    public partial class UserBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BlogModel",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogModel_UserId",
                table: "BlogModel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogModel_AspNetUsers_UserId",
                table: "BlogModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogModel_AspNetUsers_UserId",
                table: "BlogModel");

            migrationBuilder.DropIndex(
                name: "IX_BlogModel_UserId",
                table: "BlogModel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogModel");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");
        }
    }
}
