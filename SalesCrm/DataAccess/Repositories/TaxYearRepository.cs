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

    public IEnumerable<TaxYear> GetTaxYearListAsync()
    {
        return _context.TaxYears;
    }

    public async Task<TaxYear> GetTaxYearByIdAsync(Guid taxId)
    {
        return await _context.TaxYears
            .Where(n => n.Id == taxId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public IEnumerable<SelectListItem> GetSelectTaxYearListAsync()
    {
        return GetTaxYearListAsync().Select(taxYear => new SelectListItem
        {
            Text = taxYear.YearOfTax,
            Value = taxYear.Id.ToString()
        }).ToList();
    }
}
