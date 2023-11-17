using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SalesCrm.Domains.Identities;

namespace SalesCrm.Domains.Entities;

public class News
{
    [Key]
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
    
    public DateTime PublishedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public bool IsActive { get; set; }
    
    [ForeignKey("AuthorId")]
    public string? AuthorId { get; set; }
    
    public User? Author { get; set; }
}
