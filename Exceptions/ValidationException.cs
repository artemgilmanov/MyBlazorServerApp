namespace MyBlazorServerApp.Exceptions;

public class ValidationException : Exception
{
  public Dictionary<string, List<string>> Errors { get; } = [];

  public ValidationException(string message, Dictionary<string, List<string>> errors)
      : base(message)
  {
    Errors = errors;
  }
}
