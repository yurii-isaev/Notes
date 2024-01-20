using Microsoft.AspNetCore.Mvc.Rendering;
using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface ITaxYearService
{
    Task CreateTaxYearAsync(TaxYearDto dto);
    Task<IEnumerable<TaxYearDto>> GetTaxYearList();
    Task<IEnumerable<SelectListItem>> GetSelectTaxListAsync();
    decimal GetTotalTax(decimal totalEarnings);
    Task DeleteTaxYearAsync(Guid id);
}
