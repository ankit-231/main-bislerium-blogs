using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bislerium_blogs.Migrations
{
    /// <inheritdoc />
    public partial class HistoryLogUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "HistoryLog",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryLog_UserId",
                table: "HistoryLog",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryLog_AspNetUsers_UserId",
                table: "HistoryLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryLog_AspNetUsers_UserId",
                table: "HistoryLog");

            migrationBuilder.DropIndex(
                name: "IX_HistoryLog_UserId",
                table: "HistoryLog");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "HistoryLog",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");
        }
    }
}
