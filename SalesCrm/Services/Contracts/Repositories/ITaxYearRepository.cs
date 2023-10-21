using Microsoft.AspNetCore.Mvc.Rendering;
using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts.Repositories;

public interface ITaxYearRepository
{
    Task CreateTaxYearAsync(TaxYear taxYear);

    IEnumerable<TaxYear> GetTaxYearListAsync();

    Task<TaxYear> GetTaxYearByIdAsync(Guid id);

    IEnumerable<SelectListItem> GetSelectTaxYearListAsync();
}
