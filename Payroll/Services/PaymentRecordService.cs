using AutoMapper;
using Payroll.Domains.Entities;
using Payroll.Services.Contracts.Repositories;
using Payroll.Services.Contracts.Services;
using Payroll.Services.Objects;

namespace Payroll.Services;

public class PaymentRecordService : IPaymentRecordService
{
    readonly IMapper _mapper;
    readonly IPaymentRecordRepository _repository;

    public PaymentRecordService(IMapper mapper, IPaymentRecordRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task CreatePaymentRecord(PaymentRecordDto dto)
    {
        var paymentRecord = _mapper.Map<PaymentRecord>(dto);
        await _repository.CreatePaymentRecordAsync(paymentRecord);
    }

    public async Task<IEnumerable<PaymentRecordDto>> GetPaymentRecordListAsync(string keyword)
    {
        try
        {
            var taxYearList = await _repository.GetPaymentRecordList();
            
            if (!string.IsNullOrEmpty(keyword))
            {
                taxYearList = taxYearList.Where(rec => rec.Employee!.Name!.Contains(keyword));
            }
            
            return _mapper.Map<IEnumerable<PaymentRecordDto>>(taxYearList);
        }
        catch (Exception)
        {
            return Enumerable.Empty<PaymentRecordDto>();
        }
    }

    public async Task<PaymentRecordDto> GetPaymentRecordByIdAsync(Guid paymentRecordId)
    {
        var paymentRecord = await _repository.GetPaymentRecordAsync(paymentRecordId);
        return _mapper.Map<PaymentRecordDto>(paymentRecord);
    }

    public async Task DeletePaymentRecordAsync(Guid paymentRecordId)
    {
        await _repository.DeletePaymentRecordAsync(paymentRecordId);
    }
}
