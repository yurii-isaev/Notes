using Microsoft.EntityFrameworkCore;
using SalesCrm.Domains.Entities;
using SalesCrm.Services.Contracts.Repositories;

namespace SalesCrm.DataAccess.Repositories;

public class PaymentRecordRepository : IPaymentRecordRepository
{
    readonly EmployeeDbContext _context;

    public PaymentRecordRepository(EmployeeDbContext context) => _context = context;

    public async Task CreatePaymentRecordAsync(PaymentRecord paymentRecord)
    {
        await _context.PaymentRecords.AddAsync(paymentRecord);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PaymentRecord>> GetPaymentRecordList()
    {
        return await _context.PaymentRecords
            .Include(n => n.Employee)
            .Include(m => m.TaxYear)
            .ToListAsync() ?? throw new InvalidOperationException();
    }

    public async Task<PaymentRecord> GetPaymentRecordAsync(Guid paymentRecordId)
    {
        return await _context.PaymentRecords
            .Include(pr => pr.Employee)
            .Include(pr => pr.TaxYear)
            .Where(pr => pr.Id == paymentRecordId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    public async Task DeletePaymentRecordAsync(Guid paymentRecordId)
    {
        var paymentRecord = await _context.PaymentRecords
            .Where(n => n.Id == paymentRecordId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
        
        _context.PaymentRecords.Remove(paymentRecord);
        await _context.SaveChangesAsync();
    }
}
