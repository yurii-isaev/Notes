namespace Payroll.Pages.Shared.Pagination;

public class PaginationViewModel
{
    public string? ActionName { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool IsPreviousPageAvailable { get; set; }
    public bool IsNextPageAvailable { get; set; }
    public int TotalPages { get; set; }
}