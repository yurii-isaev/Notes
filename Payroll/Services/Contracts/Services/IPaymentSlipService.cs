namespace Payroll.Services.Contracts.Services;

public interface IPaymentSlipService
{
    decimal GetOvertimeHorse(decimal hoursWorked, decimal contractualHours);

    decimal GetContractualEarnings(decimal hoursWorked, decimal contractualHours, decimal hourlyRate);

    decimal GetOvertimeEarning(decimal overtimeHours, decimal overtimeRate);

    decimal GetOvertimeRate(decimal hourlyRate);
    
    decimal GetTotalEarning(decimal overtimeEarnings, decimal contractualEarnings);

    Task<decimal> GetTotalDeductionAsync(decimal unionFree, decimal tax);
    
    Task<decimal> GetNetPaymentAsync(decimal totalEarnings, decimal totalDeduction);
}
