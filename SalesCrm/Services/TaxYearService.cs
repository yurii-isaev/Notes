using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class TaxYearService : ITaxYearService
{
    private readonly ILogger<TaxYearService> _logger;
    private readonly IMapper _mapper;
    private readonly ITaxYearRepository _repository;

    public TaxYearService(ITaxYearRepository repository, IMapper mapper, ILogger<TaxYearService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task CreateTaxYearAsync(TaxYearDto dto)
    {
        try
        {
            var taxYear = _mapper.Map<TaxYear>(dto);
            await _repository.CreateTaxYearAsync(taxYear);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Exception create Tax Year]: " + ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<TaxYearDto>> GetTaxYearList()
    {
        try
        {
            var taxYearList = await _repository.GetTaxYearListAsync();
            return _mapper.Map<IEnumerable<TaxYearDto>>(taxYearList);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Get tax year list]: " + ex.Message);
            return Enumerable.Empty<TaxYearDto>();
        }
    }

    public Task<TaxYear> GetTaxYearByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SelectListItem>> GetSelectTaxListAsync()
    {
        try
        {
            return await _repository.GetSelectTaxYearListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("[Get Select Tax Year]: " + ex.Message);
            throw;
        }
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
