using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Payroll.Domains.Identities;

namespace Payroll.Domains.Entities;

public class News
{
  [Key]
  public Guid Id { get; set; }

  public string? Title { get; set; }

  public string? Description { get; set; }

  public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  public bool IsActive { get; set; }

  [ForeignKey("AuthorId")]
  public string? AuthorId { get; set; }

  public User? Author { get; set; }
}
