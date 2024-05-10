using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bislerium_blogs.Migrations
{
    /// <inheritdoc />
    public partial class JustIsCurrentBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTimestamp",
                table: "BlogModel",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isCurrent",
                table: "BlogModel",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedTimestamp",
                table: "BlogModel");

            migrationBuilder.DropColumn(
                name: "isCurrent",
                table: "BlogModel");
        }
    }
}
