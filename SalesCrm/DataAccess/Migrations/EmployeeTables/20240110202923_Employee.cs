using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesCrm.DataAccess.Migrations.EmployeeTables
{
    /// <inheritdoc />
    public partial class Employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    ImageName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Designation = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    InsuranceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    StudentLoanStatus = table.Column<bool>(type: "boolean", nullable: false),
                    UnionMemberStatus = table.Column<bool>(type: "boolean", nullable: false),
                    Address = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Postcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    YearOfTax = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    InsuranceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PayDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PayMonth = table.Column<string>(type: "text", nullable: true),
                    TaxYearId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaxCode = table.Column<string>(type: "text", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "money", nullable: false),
                    HoursWorked = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ContractualHours = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OvertimeHours = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ContractualEarnings = table.Column<decimal>(type: "money", nullable: false),
                    OvertimeEarnings = table.Column<decimal>(type: "money", nullable: false),
                    Tax = table.Column<decimal>(type: "money", nullable: false),
                    Nic = table.Column<decimal>(type: "money", nullable: false),
                    UnionFree = table.Column<decimal>(type: "money", nullable: true),
                    TotalEarnings = table.Column<decimal>(type: "money", nullable: false),
                    TotalDeduction = table.Column<decimal>(type: "money", nullable: false),
                    NetPayment = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentRecords_TaxYears_TaxYearId",
                        column: x => x.TaxYearId,
                        principalTable: "TaxYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecords_EmployeeId",
                table: "PaymentRecords",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecords_TaxYearId",
                table: "PaymentRecords",
                column: "TaxYearId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentRecords");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "TaxYears");
        }
    }
}
