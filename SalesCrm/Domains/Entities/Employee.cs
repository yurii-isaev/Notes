using System.ComponentModel.DataAnnotations;

namespace SalesCrm.Domains.Entities;

public class Employee
{
    [Key]
    public Guid Id { get; set; }
    
    public string? Name { get; set; }

    public string? Gender { get; set; }
    
    public string? ImageName { get; set; }
    
    public DateTime DateOfBirth { get; set; }

    public DateTime DateJoined { get; set; }

    public string? Phone { get; set; }

    public string? Designation { get; set; }

    public string? Email { get; set; }

    public string? InsuranceNumber { get; set; }

    public string? PaymentMethod { get; set; }

    public bool StudentLoanStatus { get; set; }

    public bool UnionMemberStatus { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Postcode { get; set; }
    
    public IEnumerable<PaymentRecord>? PaymentRecord { get; set; }
}
