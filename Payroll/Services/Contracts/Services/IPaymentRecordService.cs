using Payroll.Services.Objects;

namespace Payroll.Services.Contracts.Services;

public interface IPaymentRecordService
{
  Task CreatePaymentRecord(PaymentRecordDto dto);

  Task<IEnumerable<PaymentRecordDto>> GetPaymentRecordListAsync(string keyword);

  Task<PaymentRecordDto> GetPaymentRecordByIdAsync(Guid paymentRecordId);

  Task DeletePaymentRecordAsync(Guid paymentRecordId);
}
