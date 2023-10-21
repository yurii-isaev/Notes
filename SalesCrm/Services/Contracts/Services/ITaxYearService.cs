using Microsoft.AspNetCore.Mvc.Rendering;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface ITaxYearService
{
    Task CreateTaxYearAsync(TaxYearDto dto);
    
    IEnumerable<TaxYearDto> GetTaxYearList();
    
    Task<TaxYear> GetTaxYearByIdAsync(Guid id);
    
    Task<IEnumerable<SelectListItem>> GetSelectTaxListAsync();
}
