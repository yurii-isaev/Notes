using Microsoft.AspNetCore.Mvc.Rendering;
using Payroll.Services.Objects;

namespace Payroll.Services.Contracts.Services;

public interface ITaxYearService
{
    Task CreateTaxYearAsync(TaxYearDto dto);
    
    Task<IEnumerable<TaxYearDto>> GetTaxYearListAsync();
    
    Task<IEnumerable<SelectListItem>> GetSelectTaxListAsync();
    
    decimal GetTotalTax(decimal totalEarnings);
    
    Task DeleteTaxYearAsync(Guid id);
}
