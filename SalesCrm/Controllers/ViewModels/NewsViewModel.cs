using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SalesCrm.Domains.Identities;

namespace SalesCrm.Controllers.ViewModels;

public class NewsViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required"),
     StringLength(50, MinimumLength = 2, ErrorMessage = "Invalid Title length")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Text is required")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Create date is required")]
    public DateTime CreatedAt { get; set; }

    public DateTime PublishedAt { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public string? AuthorId { get; set; }

    public User? Author { get; set; }
}
