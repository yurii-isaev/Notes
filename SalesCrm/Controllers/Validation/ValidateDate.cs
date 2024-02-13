using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SalesCrm.Controllers.Validation;

public class ValidateDate : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string inputValue)
        {
            return new ValidationResult("Invalid input type.");
        }

        // Checking the input format (must be = XXXX)
        if (!Regex.IsMatch(inputValue, @"^\d{4}$"))
        {
            return new ValidationResult("Invalid input format. Please use XXXX format.");
        }

        // Checking that the year is not the future
        var year = int.Parse(inputValue);
        if (year > DateTime.Now.Year)
        {
            return new ValidationResult($"Year {year} has not yet come.");
        }

        // All checks passed successfully
        return ValidationResult.Success!;
    }
}
