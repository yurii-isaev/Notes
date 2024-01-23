using AutoMapper;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;
using SalesCrm.Services.Contracts.Services;
using SalesCrm.Services.Input;

namespace SalesCrm.Services;

public class PaymentRecordService : IPaymentRecordService
{
    private readonly ILogger<TaxYearService> _logger;
    private readonly IMapper _mapper;
    private readonly IPaymentRecordRepository _repository;

    public PaymentRecordService(IPaymentRecordRepository repository, IMapper mapper, ILogger<TaxYearService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task CreatePaymentRecord(PaymentRecordDto dto)
    {
        try
        {
            var paymentRecord = _mapper.Map<PaymentRecord>(dto);
            await _repository.CreatePaymentRecordAsync(paymentRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Exception create Payment Record]: " + ex.Message);
        }
    }

    public async Task<IEnumerable<PaymentRecordDto>> GetPaymentRecordList()
    {
        try
        {
            var taxYearList = await _repository.GetPaymentRecordList();
            return _mapper.Map<IEnumerable<PaymentRecordDto>>(taxYearList);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Get Payment Record List]: " + ex.Message);
            return Enumerable.Empty<PaymentRecordDto>();
        }
    }

    public async Task<PaymentRecordDto> GetPaymentRecordByIdAsync(Guid paymentRecordId)
    {
        try
        {
            var paymentRecord = await _repository.GetPaymentRecordAsync(paymentRecordId);
            return _mapper.Map<PaymentRecordDto>(paymentRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError("[Get Employee Payment Record]: " + ex.Message);
            throw;
        }
    }
}
