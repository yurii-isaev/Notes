using SalesCrm.Services.Input;

namespace SalesCrm.Services.Contracts.Services;

public interface IPaymentRecordService
{
    Task CreatePaymentRecord(PaymentRecordDto dto);

    Task<IEnumerable<PaymentRecordDto>> GetPaymentRecordList();

    Task<PaymentRecordDto> GetPaymentRecordByIdAsync(Guid paymentRecordId);
}
