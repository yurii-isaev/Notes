using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SalesCrm.Controllers.Validation;

public class ValidateDate : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        // Ensure that the value is not empty or whitespace
        if (string.IsNullOrWhiteSpace((string?) value))
        {
            return new ValidationResult("The Tax Year field is required.");
        }
        
        // Check if the value is provided and meets the required length
        if (value.ToString()!.Length != 4)
        {
            return new ValidationResult("Invalid input year. Please enter a 4-digit year.");
        }

        // Checking the input format (must be = XXXX)
        if (!Regex.IsMatch(value.ToString()!, @"^\d{4}$"))
        {
            return new ValidationResult("Invalid input format. Please use XXXX format.");
        }

        // Checking that the year is not the future
        var year = int.Parse(value.ToString()!);
        if (year > DateTime.Now.Year)
        {
            return new ValidationResult($"Year {year} has not yet come.");
        }

        // All checks passed successfully
        return ValidationResult.Success!;
    }
}
