using SalesCrm.Services.Contracts.Services;

namespace SalesCrm.Services;

public class PaymentSlipService : IPaymentSlipService
{
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
}
