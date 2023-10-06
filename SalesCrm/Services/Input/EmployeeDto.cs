using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Enums;

namespace SalesCrm.Services.Input;

public class EmployeeDto : Employee
{
    public new IFormFile? ImageUrl { get; set; }
    public new PaymentMethod PaymentMethod { get; set; }
}
