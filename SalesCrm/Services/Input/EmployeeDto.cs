using SalesCrm.Domains.Entities;
using SalesCrm.Domains.Enums;

namespace SalesCrm.Services.Input;

public class EmployeeDto : Employee
{
    public IFormFile? FormFile { get; set; }
    public new PaymentMethod PaymentMethod { get; set; }
}
