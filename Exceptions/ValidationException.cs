namespace MyBlazorServerApp.Exceptions;

/// <summary>
/// Represents a validation exception that occurs when business rule or data validation fails.
/// This exception contains detailed error information organized by field/property names.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ValidationException"/> class.
/// </remarks>
public class ValidationException : Exception
{
  /// <summary>
  /// Gets a dictionary containing validation errors where the key is the field/property name
  /// and the value is a list of error messages for that field.
  /// </summary>
  /// <value>
  /// A dictionary mapping field names to lists of validation error messages.
  /// </value>
  public Dictionary<string, List<string>> Errors { get; } = [];

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidationException"/> class with a specified error message
  /// and validation errors dictionary.
  /// </summary>
  /// <param name="message">The error message that explains the reason for the exception.</param>
  /// <param name="errors">A dictionary containing validation errors organized by field names.</param>
  public ValidationException(string message, Dictionary<string, List<string>> errors)
      : base(message)
  {
    Errors = errors;
  }
}
