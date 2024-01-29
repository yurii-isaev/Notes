using AutoMapper;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

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

    public async Task<IEnumerable<PaymentRecordDto>> GetPaymentRecordList()
    {
        try
        {
            var taxYearList = await _repository.GetPaymentRecordList();
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
