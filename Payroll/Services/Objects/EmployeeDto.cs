using Payroll.Domains.Entities;
using Payroll.Domains.Enums;

namespace Payroll.Services.Objects;

public class EmployeeDto : Employee
{
  public IFormFile? FormFile { get; set; }

  public new PaymentMethod PaymentMethod { get; set; }
}
