using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Payroll.Domains.Enums;

namespace Payroll.Controllers.ViewModels;

public class EmployeeViewModel
{
    [HiddenInput(DisplayValue = false)]
    public Guid Id { get; set; }

    [
        Required,
        StringLength(50, MinimumLength = 5, ErrorMessage = "Invalid Name Length.")
    ]
    public string? Name { get; set; }

    [Required]
    public string? Gender { get; set; }

    public IFormFile? FormFile { get; set; }

    public string? ImageName { get; set; }

    [
        DataType(DataType.Date),
        DisplayName("Date Of Birth"),
        Required,
    ]
    public DateTime DateOfBirth { get; set; }

    [
        DataType(DataType.Date),
        DisplayName("Joining Date"),
        Required
    ]
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

    [
        Required,
        StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid field length.")
    ]
    public string? Phone { get; set; }

    [
        Required,
        StringLength(100, MinimumLength = 5, ErrorMessage = "Invalid field length.")
    ]
    public string? Designation { get; set; }

    [
        DataType(DataType.EmailAddress),
        Required
    ]
    public string? Email { get; set; }

    [
        DisplayName("Insurance Number"),
        Required,
        StringLength(9, MinimumLength = 9, ErrorMessage = "Invalid field length."),
        RegularExpression(@"^[A-CEGHJ-PR-TW-Z]{1}[A-CEGHJ-NPR-TW-Z]{1}[0-9]{6}[A-D\s]$")
    ]
    public string? InsuranceNumber { get; set; }

    [
        DisplayName("Payment Method"),
        Required
    ]
    public PaymentMethod PaymentMethod { get; set; }

    [
        DisplayName("Union Member Status"),
        Required
    ]
    public bool UnionMemberStatus { get; set; }

    [
        DisplayName("Student Loan Status"),
        Required
    ]
    public bool StudentLoanStatus { get; set; }

    [
        Required,
        StringLength(200, MinimumLength = 5, ErrorMessage = "Invalid field length.")
    ]
    public string? Address { get; set; }

    [
        Required,
        StringLength(150, MinimumLength = 5, ErrorMessage = "Invalid field length.")
    ]
    public string? City { get; set; }

    [
        Required,
        StringLength(50, MinimumLength = 9, ErrorMessage = "Invalid field length.")
    ]
    public string? Postcode { get; set; }
}
