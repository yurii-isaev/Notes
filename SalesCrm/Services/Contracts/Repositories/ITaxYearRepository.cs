using Microsoft.AspNetCore.Mvc.Rendering;
using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts.Repositories;

public interface ITaxYearRepository
{
    Task CreateTaxYearAsync(TaxYear taxYear);
    Task<IEnumerable<TaxYear>> GetTaxYearListAsync();
    Task<TaxYear> GetTaxYearByIdAsync(Guid id);
    Task<IEnumerable<SelectListItem>> GetSelectTaxYearListAsync();
    bool IsTaxYearExists(string dtoYearOfTax);
    Task DeleteTaxYearAsync(Guid id);
}
