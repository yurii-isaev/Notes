using SalesCrm.Domains.Entities;

namespace SalesCrm.Services.Contracts.Repositories;

public interface IPaymentRecordRepository
{
    Task CreatePaymentRecordAsync(PaymentRecord paymentRecord);
    Task<IEnumerable<PaymentRecord>> GetPaymentRecordList();
    Task<PaymentRecord> GetPaymentRecordAsync(Guid paymentRecordId);
    Task DeletePaymentRecordAsync(Guid paymentRecordId);
}
