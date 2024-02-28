namespace Payroll.Services.Contracts.Services;

public interface IDeductionService
{
    Task<decimal> GetTaxDeduction(decimal totalEarnings);

    Task<decimal> GetUnionFree(bool unionMemberStatus);

    Task<decimal> GetTotalDeductionAsync(decimal unionFree, decimal tax);
}
