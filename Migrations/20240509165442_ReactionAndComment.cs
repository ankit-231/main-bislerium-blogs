using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace bislerium_blogs.Migrations
{
    /// <inheritdoc />
    public partial class ReactionAndComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentModel_BlogModel_BlogId",
                        column: x => x.BlogId,
                        principalTable: "BlogModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReactionModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ReactionStatus = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReactionModel_BlogModel_BlogId",
                        column: x => x.BlogId,
                        principalTable: "BlogModel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReactionModel_CommentModel_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CommentModel",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_BlogId",
                table: "CommentModel",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentModel_UserId",
                table: "CommentModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionModel_BlogId",
                table: "ReactionModel",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionModel_CommentId",
                table: "ReactionModel",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionModel_UserId",
                table: "ReactionModel",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactionModel");

            migrationBuilder.DropTable(
                name: "CommentModel");
        }
    }
}
