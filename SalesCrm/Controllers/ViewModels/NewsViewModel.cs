namespace SalesCrm.Controllers.ViewModels;

public class NewsViewModel
{
    public string? Title { get; set; }

    public string? Text { get; set; }

    public DateTime Date { get; set; }

    public string? AuthorId { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; }
}
