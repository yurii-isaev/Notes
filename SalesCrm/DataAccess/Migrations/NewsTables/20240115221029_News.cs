using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesCrm.DataAccess.Migrations.NewsTables
{
    /// <inheritdoc />
    public partial class News : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "AspNetUsers",
            //     columns: table => new
            //     {
            //         Id = table.Column<string>(type: "text", nullable: false),
            //         Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //         UserName = table.Column<string>(type: "text", nullable: true),
            //         NormalizedUserName = table.Column<string>(type: "text", nullable: true),
            //         Email = table.Column<string>(type: "text", nullable: true),
            //         NormalizedEmail = table.Column<string>(type: "text", nullable: true),
            //         EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
            //         PasswordHash = table.Column<string>(type: "text", nullable: true),
            //         SecurityStamp = table.Column<string>(type: "text", nullable: true),
            //         ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
            //         PhoneNumber = table.Column<string>(type: "text", nullable: true),
            //         PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
            //         TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
            //         LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
            //         LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
            //         AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //     }
            // );

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    AuthorId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_AuthorId",
                table: "News",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "News");
            // migrationBuilder.DropTable(name: "AspNetUsers");
        }
    }
}
