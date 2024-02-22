#nullable disable
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.DataAccess.Migrations.NewsTables
{
    /// <inheritdoc />
    public partial class NewsAddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "News",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "News");
        }
    }
}
