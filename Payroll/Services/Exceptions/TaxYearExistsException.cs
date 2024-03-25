namespace Payroll.Services.Exceptions;

public class TaxYearExistsException : Exception
{
  public TaxYearExistsException()
  {}

  public TaxYearExistsException(string message) : base(message)
  {}

  public TaxYearExistsException(string message, Exception innerException) : base(message, innerException)
  {}
}
