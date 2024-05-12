using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bislerium_blogs.Migrations
{
    /// <inheritdoc />
    public partial class SelfTry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "BlogModel");

            migrationBuilder.DropColumn(
                name: "UpdatedTimestamp",
                table: "BlogModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
