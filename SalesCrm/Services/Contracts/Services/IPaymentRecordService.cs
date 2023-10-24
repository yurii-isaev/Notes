namespace SalesCrm.Services.Contracts.Services;

public interface IPaymentRecordService
{
    decimal GetOvertimeHorse(decimal hoursWorked, decimal contractualHours);

    decimal GetContractualEarnings(decimal hoursWorked, decimal contractualHours, decimal hourlyRate);

    decimal GetOvertimeEarning(decimal overtimeHours, decimal overtimeRate);

    decimal GetOvertimeRate(decimal hourlyRate);
    decimal GetTotalEarning(decimal overtimeEarnings, decimal contractualEarnings);
}

