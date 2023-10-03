using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SalesCrm.Domains.Enums;

namespace SalesCrm.Controllers.ViewModels;

public class EmployeeViewModel
{
    [HiddenInput(DisplayValue = false)]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Name is required"), 
     StringLength(50, MinimumLength = 2, ErrorMessage = "Invalid Name Length")]
    public string? Name { get; set; }
    
    public string? Gender { get; set; }
    
    [DisplayName("Photo")]
    [Required]
    public IFormFile? ImageUrl { get; set; }
    
    [DataType(DataType.Date), DisplayName("Date Of Birth")]
    public DateTime DateOfBirth { get; set; }

    [DataType(DataType.Date), DisplayName("Joining Date")]
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;
    
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Designation is required"), StringLength(100)]
    public string? Designation { get; set; }

    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required, StringLength(50)]
    [RegularExpression(@"^[A-CEGHJ-PR-TW-Z]{1}[A-CEGHJ-NPR-TW-Z]{1}[0-9]{6}[A-D\s]$")]
    public string? Insurance { get; set; }

    [DisplayName("Payment Method")]
    public PaymentMethod PaymentMethod { get; set; }

    [DisplayName("Union Member Status")]
    public bool UnionMemberStatus { get; set; }
    
    [DisplayName("Student Loan Status")]
    public bool StudentLoanStatus { get; set; }
    
    [Required, StringLength(150)]
    public string? Address { get; set; }
    
    [Required, StringLength(50)]
    public string? City { get; set; }
    
    public string? Postcode { get; set; }
}
