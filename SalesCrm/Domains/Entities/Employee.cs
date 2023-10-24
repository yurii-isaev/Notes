using System.ComponentModel.DataAnnotations;

namespace SalesCrm.Domains.Entities;

public class Employee
{
    public Guid Id { get; set; }
    
    [Required, MaxLength(50)]
    public string? Name { get; set; }
    
    [Required]
    public string? Gender { get; set; }
    
    public string? ImageName { get; set; }
    
    public DateTime DateOfBirth { get; set; }

    public DateTime DateJoined { get; set; }

    [Required]
    public string? Phone { get; set; }

    [Required]
    public string? Designation { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required, MaxLength(50)]
    public string? InsuranceNumber { get; set; }

    [Required]
    public string? PaymentMethod { get; set; }
    
    [Required]
    public bool StudentLoanStatus { get; set; }
    
    [Required]
    public bool UnionMemberStatus { get; set; }

    [Required, MaxLength(150)]
    public string? Address { get; set; }
    
    [Required, MaxLength(50)]
    public string? City { get; set; }
    
    [Required, MaxLength(50)]
    public string? Postcode { get; set; }
    
    public IEnumerable<PaymentRecord>? PaymentRecord { get; set; }
}
