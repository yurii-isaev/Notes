using Payroll.Services.Contracts.Services;

namespace Payroll.Services;

public class DeductionService : IDeductionService
{
 
    
    // public decimal GetTotalTax(decimal totalEarnings)
    // {
    //     decimal totalTax = 0m;
    //     decimal taxRate = 0m;
    //
    //     if (totalEarnings < 250000)
    //     {
    //         taxRate = .0m;
    //         totalTax = totalEarnings * taxRate;
    //     }
    //     else if (totalEarnings > 250000 && totalEarnings <= 500000)
    //     {
    //         taxRate = .50m;
    //         totalTax = (totalEarnings - 250000) * taxRate;
    //     }
    //     else if (totalEarnings > 500000 && totalEarnings <= 1000000)
    //     {
    //         taxRate = 1m;
    //         totalTax = ((totalEarnings - 250000) * .50m) + ((totalEarnings - 500000) * taxRate);
    //     }
    //
    //     return totalTax;
    // }
    
    public Task<decimal> GetTaxDeduction(decimal totalEarnings)
    {
        decimal slab1 = 14000m;
        decimal slab1Percent = .105m;
        decimal slab2 = 48000m;
        decimal slab2Percent = .175m;
        decimal slab3 = 70000m;
        decimal slab3Percent = .300m; 
        decimal maxPercent = .33m;
        decimal taxDeduction = 0m;
        decimal taxRate = 0m;

        if (totalEarnings <= slab1)
        {
            taxRate = slab1Percent;
            taxDeduction = totalEarnings * slab1Percent;
        }
        else if (totalEarnings > slab1 && totalEarnings <= slab2)
        {
            taxRate = slab2Percent;
            taxDeduction = totalEarnings * slab2Percent;
        }
        else if (totalEarnings > slab2 && totalEarnings <= slab3)
        {
            taxRate = slab3Percent;
            taxDeduction = totalEarnings * slab3Percent;
        }
        else if (totalEarnings > slab3)
        {
            taxRate = maxPercent;
            taxDeduction = totalEarnings * maxPercent;
        }

        return Task.FromResult(taxDeduction);
    }
    
    public Task<decimal> GetUnionFree(bool unionMemberStatus)
    {
        return Task.FromResult((unionMemberStatus) ? 100m : 0);
    }
    
    public Task<decimal> GetTotalDeductionAsync(decimal unionFree, decimal tax)
    {
        return Task.FromResult(unionFree + tax);
    }
}
