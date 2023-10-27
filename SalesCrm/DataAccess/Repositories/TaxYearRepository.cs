using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;

namespace SalesCrm.DataAccess.Repositories;

public class TaxYearRepository : ITaxYearRepository
{
    private EmployeeDbContext _context;

    public TaxYearRepository(EmployeeDbContext context) => _context = context;

    public async Task CreateTaxYearAsync(TaxYear taxYear)
    {
        await _context.TaxYears.AddAsync(taxYear);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<TaxYear>> GetTaxYearListAsync()
    {
        return Task.FromResult<IEnumerable<TaxYear>>(_context.TaxYears);
    }

    public async Task<TaxYear> GetTaxYearByIdAsync(Guid taxId)
    {
        return await _context.TaxYears
            .Where(n => n.Id == taxId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public Task<IEnumerable<SelectListItem>> GetSelectTaxYearListAsync()
    {
        return Task.FromResult<IEnumerable<SelectListItem>>(GetTaxYearListAsync()
            .Result.Select(taxYear => new SelectListItem
            {
                Text = taxYear.YearOfTax,
                Value = taxYear.Id.ToString()
            }).ToList()
        );
    }
}
