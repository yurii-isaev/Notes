using System.ComponentModel.DataAnnotations;
using SalesCrm.Domains.Enums;

namespace SalesCrm.Domains.Entities;

public class Employee
{
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string? Name { get; set; }
    
    public string? Gender { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public DateTime DateOfBirth { get; set; }

    public DateTime DateJoined { get; set; }

    public string? Phone { get; set; }

    public string? Designation { get; set; }

    public string? Email { get; set; }

    [Required, MaxLength(50)]
    public string? Insurance { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public StudentLoanStatus StudentLoanStatus { get; set; }
    
    public UnionMemberStatus UnionMemberStatus { get; set; }

    [Required, MaxLength(150)]
    public string? Address { get; set; }
    
    [Required, MaxLength(50)]
    public string? City { get; set; }
    
    [Required, MaxLength(50)]
    public string? Postcode { get; set; }
    
    public IEnumerable<PaymentRecord>? PaymentRecord { get; set; }
}
