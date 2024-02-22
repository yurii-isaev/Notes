using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Payroll.Domains.Identities;

namespace Payroll.Controllers.ViewModels;

public class NewsViewModel
{
    [HiddenInput(DisplayValue = false)]
    public Guid Id { get; set; }

    [
        Required,
        StringLength(50, MinimumLength = 4, ErrorMessage = "Invalid Title length")
    ]
    public string? Title { get; set; }

    [
        Required,
        StringLength(100, MinimumLength = 5, ErrorMessage = "Invalid Title length")
    ]
    public string? Description { get; set; }

    [
        DisplayName("Created date"),
        Required
    ]
    public DateTime CreatedAt { get; set; }

    [
        DisplayName("Published date"),
        Required
    ]
    public DateTime PublishedAt { get; set; }

    [
        DisplayName("Updated date"),
        Required
    ]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsActive { get; set; } = true;

    public string? AuthorId { get; set; }

    public User? Author { get; set; }
}
