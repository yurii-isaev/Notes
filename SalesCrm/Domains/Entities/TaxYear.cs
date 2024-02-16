using System.ComponentModel.DataAnnotations;

namespace SalesCrm.Domains.Entities;

public class TaxYear
{
    [Key]
    public Guid Id { get; set; }

    public string? YearOfTax { get; set; }
}
