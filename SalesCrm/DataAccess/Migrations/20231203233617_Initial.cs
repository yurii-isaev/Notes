using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable
namespace SalesCrm.Migrations;

public partial class Initial : Migration 
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(name: "SalesLeads", columns: table => new
            { 
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn), 
                FirstName = table.Column<string>(type: "text", nullable: false),
                LastName = table.Column<string>(type: "text", nullable: false),
                Mobile = table.Column<string>(type: "text", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                Source = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_SalesLeads", x => x.Id));
    } 
    
    protected override void Down(MigrationBuilder migrationBuilder) 
    {
        migrationBuilder.DropTable(name: "SalesLeads");
    }
}

