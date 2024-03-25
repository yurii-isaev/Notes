using System.ComponentModel.DataAnnotations;

namespace Payroll.Domains.Entities;

public class TaxYear
{
  [Key]
  public Guid Id { get; set; }

  public string? YearOfTax { get; set; }
}
