using SalesCrm.Domains.Entities;

namespace SalesCrm.Controllers.ViewModels;

public class PaymentRecordViewModel
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Employee? Employee { get; set; }

    public string? Name { get; set; }
    
    public string? InsuranceNumber { get; set; }

    public DateTime PayDate { get; set; } = DateTime.UtcNow;

    public string? PayMonth { get; set; } = DateTime.Today.Month.ToString();

    public Guid TaxYearId { get; set; }

    public TaxYear? TaxYear { get; set; }

    public string? TaxCode { get; set; } = "100L";

    public decimal HourlyRate { get; set; }

    public decimal HoursWorked { get; set; }

    public decimal ContractualHours { get; set; } = 150;

    public decimal OvertimeHours { get; set; }

    public decimal ContractualEarnings { get; set; }

    public decimal OvertimeEarnings { get; set; }

    public decimal Tax { get; set; }

    public decimal Nic { get; set; }

    public decimal UnionFree { get; set; }

    public decimal TotalEarnings { get; set; }

    public decimal TotalDeduction { get; set; }

    public decimal NetPayment { get; set; }
}
