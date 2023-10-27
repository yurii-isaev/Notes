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
    
    public decimal GetContractualEarnings(decimal hoursWorked, decimal contractualHours, decimal hourlyRate)
    {
        decimal contractualEarnings = 0;

        if (hoursWorked < contractualHours)
        {
            contractualEarnings = hourlyRate * hoursWorked;
        }
        else
        {
            contractualEarnings = hourlyRate * contractualHours;
        }

        return contractualEarnings;
    }

    public decimal GetOvertimeHorse(decimal hoursWorked, decimal contractualHours)
    {
        decimal overtimeHours = 0;

        if (hoursWorked <= contractualHours)
        {
            overtimeHours = 0.0m;
        }
        else if (hoursWorked > contractualHours)
        {
            overtimeHours = hoursWorked - contractualHours;
        }

        return overtimeHours;
    }

    public decimal GetOvertimeEarning(decimal overtimeHours, decimal overtimeRate)
    {
        return (overtimeHours * overtimeRate);
    }

    public decimal GetOvertimeRate(decimal hourlyRate)
    {
       return (1.5m * hourlyRate);
    }

    public decimal GetTotalEarning(decimal overtimeEarnings, decimal contractualEarnings)
    {
        return (overtimeEarnings + contractualEarnings);
    }

    public Task<decimal> GetTotalDeductionAsync(decimal unionFree, decimal tax)
    {
        return Task.FromResult(unionFree + tax);
    }

    public Task<decimal> GetNetPaymentAsync(decimal totalEarnings, decimal totalDeduction)
    {
        return Task.FromResult(totalEarnings - totalDeduction);
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
}
