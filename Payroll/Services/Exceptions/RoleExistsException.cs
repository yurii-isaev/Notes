namespace Payroll.Services.Exceptions;

public class RoleExistsException : Exception
{
  public RoleExistsException()
  {}

  public RoleExistsException(string message) : base(message)
  {}

  public RoleExistsException(string message, Exception innerException) : base(message, innerException)
  {}
}
