using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payroll.Domains.Entities;

public class PaymentRecord
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Employee")]
    public Guid EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public string? InsuranceNumber { get; set; }
    
    public DateTime PayDate { get; set; }
    
    public string? PayMonth { get; set; }
    
    [ForeignKey("TaxYear")]
    public Guid TaxYearId { get; set; }
    public TaxYear? TaxYear { get; set; }
    
    public string? TaxCode { get; set; }
    
    [Column(TypeName = "money")]
    public decimal HourlyRate { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal HoursWorked { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ContractualHours { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal OvertimeHours { get; set; }

    [Column(TypeName = "money")]
    public decimal ContractualEarnings { get; set; }
    
    [Column(TypeName = "money")]
    public decimal OvertimeEarnings { get; set; }
    
    [Column(TypeName = "money")]
    public decimal Tax { get; set; }
    
    [Column(TypeName = "money")]
    public decimal Nic { get; set; }
    
    [Column(TypeName = "money")]
    public decimal? UnionFree { get; set; }
    
    [Column(TypeName = "money")]
    public decimal TotalEarnings { get; set; }
    
    [Column(TypeName = "money")]
    public decimal TotalDeduction { get; set; }
    
    [Column(TypeName = "money")]
    public decimal NetPayment { get; set; }
}
