using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Payroll.Domains.Entities;
using Payroll.Services.Contracts.Repositories;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Exceptions;
using Payroll.Services.Objects;

namespace Payroll.Services;

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
}
