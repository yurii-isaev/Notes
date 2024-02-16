using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Exceptions;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class TaxYearService : ITaxYearService
{
    readonly IMapper _mapper;
    readonly ITaxYearRepository _repository;

    public TaxYearService(ITaxYearRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaxYearDto>> GetTaxYearListAsync()
    {
        var taxYearList = await _repository.GetTaxYearListAsync();
        return _mapper.Map<IEnumerable<TaxYearDto>>(taxYearList);
    }

    public async Task CreateTaxYearAsync(TaxYearDto dto)
    {
        if (dto.YearOfTax != null)
        {
            if (!_repository.IsTaxYearExists(dto.YearOfTax))
            {
                var taxYear = _mapper.Map<TaxYear>(dto);
                await _repository.CreateTaxYearAsync(taxYear);
            }
            else
            {
                throw new TaxYearExistsException("This Tax Year already exists");
            }
        }
    }

    public async Task DeleteTaxYearAsync(Guid id)
    {
        await _repository.DeleteTaxYearAsync(id);
    }

    public async Task<IEnumerable<SelectListItem>> GetSelectTaxListAsync()
    {
        return await _repository.GetSelectTaxYearListAsync();
    }

    public decimal GetTotalTax(decimal totalEarnings)
    {
        decimal totalTax = 0m;
        decimal taxRate = 0m;

        if (totalEarnings < 250000)
        {
            taxRate = .0m;
            totalTax = totalEarnings * taxRate;
        }
        else if (totalEarnings > 250000 && totalEarnings < 500000)
        {
            taxRate = .50m;
            totalTax = (totalEarnings - 250000) * taxRate;
        }
        else if (totalEarnings > 500000 && totalEarnings <= 1000000)
        {
            taxRate = 1m;
            totalTax = ((totalEarnings - 250000) * .50m) + ((totalEarnings - 500000) * taxRate);
        }

        return totalTax;
    }
}
