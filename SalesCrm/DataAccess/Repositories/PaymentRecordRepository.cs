using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;

namespace SalesCrm.DataAccess.Repositories;

public class PaymentRecordRepository : IPaymentRecordRepository
{
    private EmployeeDbContext _context;

    public PaymentRecordRepository(EmployeeDbContext context) => _context = context;
    
    public async Task CreatePaymentRecordAsync(PaymentRecord paymentRecord)
    {
        await _context.PaymentRecords.AddAsync(paymentRecord);
        await _context.SaveChangesAsync();
    }
}
