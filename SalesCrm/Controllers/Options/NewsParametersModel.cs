namespace SalesCrm.Controllers.Options;

public class NewsParametersModel
{
    public string? Keyword { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public string? SortOrder { get; set; } = null;
    public string? SelectedTitle { get; set; } = null;
    public string? SelectedAuthor { get; set; } = null;
    public string? SelectedCreateDate { get; set; } = null;
    public string? SelectedPublishDate { get; set; } = null;
    public string? SelectedUpdateDate { get; set; } = null;
    public string? SelectedActivity { get; set; } = null;
}
