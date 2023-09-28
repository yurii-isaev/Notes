namespace SalesCrm.Views;

public class PaginationList<T> : List<T>
{
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PaginationList(List<T> list, int count, int pageNumber, int pageSize)
    {
        TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        PageNumber = pageNumber;
        PageSize = pageSize;
        this.AddRange(list);
    }

    public bool IsPreviousPageAvailable => PageNumber > 1;

    public bool IsNextPageAvailable => PageNumber < TotalPages;

    public static PaginationList<T> Create(IList<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count;
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginationList<T>(items, count, pageNumber, pageSize);
    }
}
