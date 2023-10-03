namespace SalesCrm.Controllers.ViewModels;

public class EmployeeListViewModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime DateJoined { get; set; }

    public string? Phone { get; set; }

    public string? Designation { get; set; }

    public string? City { get; set; }
}
