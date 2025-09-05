namespace MyBlazorServerApp.Middleware;

using MyBlazorServerApp.Exceptions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Provides helper methods for handling exceptions in a consistent manner across the application.
/// This static class contains logic for processing different types of exceptions and generating
/// appropriate HTTP responses with standardized error formats.
/// </summary>
internal static class ExceptionMiddlewareHelper
{
  /// <summary>
  /// Handles exceptions by generating appropriate HTTP responses based on the exception type.
  /// </summary>
  /// <param name="context">The HTTP context for the current request.</param>
  /// <param name="exception">The exception that was thrown during request processing.</param>
  /// <returns>A task that represents the asynchronous exception handling operation.</returns>
  /// <remarks>
  /// This method processes different types of exceptions:
  /// <list type="bullet">
  /// <item><description>ValidationException: Returns 400 Bad Request with detailed error dictionary</description></item>
  /// <item><description>Exceptions with "Validation failed:" messages: Parses and returns structured validation errors</description></item>
  /// <item><description>All other exceptions: Returns 500 Internal Server Error with generic error message</description></item>
  /// </list>
  /// </remarks>
  internal static Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    // Handle structured ValidationException
    if (exception is ValidationException validationException)
    {
      Log.Warning(
        "Validation failed for request: {RequestPath} with errors: {@Errors}",
        context.Request.Path,
        validationException.Errors);

      context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      context.Response.ContentType = "application/json";

      return context.Response.WriteAsJsonAsync(new { errors = validationException.Errors });
    }

    // Handle exceptions with validation messages in the string (e.g., from FluentValidation)
    else if (exception.Message.StartsWith("Validation failed:"))
    {
      Log.Warning(
        "Validation failed for request: {RequestPath} with message: {Message}",
        context.Request.Path,
        exception.Message);

      context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      context.Response.ContentType = "application/json";

      var errors = ParseValidationErrorMessage(exception.Message);
      return context.Response.WriteAsJsonAsync(new { errors });
    }

    // Handle all other unhandled exceptions
    else
    {
      Log.Error(
        exception,
        "An unhandled exception occurred during request: {RequestPath}",
        context.Request.Path);

      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      context.Response.ContentType = "application/json";

      var result = JsonSerializer.Serialize(new
      {
        error = "An internal server error has occurred.",
        details = exception.Message
      });

      return context.Response.WriteAsync(result);
    }
  }

  /// <summary>
  /// Parses a validation error message string into a structured dictionary of errors.
  /// </summary>
  /// <param name="errorMessage">The validation error message to parse.</param>
  /// <returns>
  /// A dictionary where keys are property names and values are lists of error messages for each property.
  /// </returns>
  /// <remarks>
  /// Expected message format:
  /// <code>
  /// Validation failed:
  ///  -- PropertyName: Error message Severity: Error
  ///  -- AnotherProperty: Another error message Severity: Error
  /// </code>
  /// </remarks>
  private static Dictionary<string, List<string>> ParseValidationErrorMessage(string errorMessage)
  {
    var errors = new Dictionary<string, List<string>>();
    var lines = errorMessage.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);

    foreach (var line in lines)
    {
      if (line.StartsWith(" -- "))
      {
        var parts = line.Substring(4).Split([": "], 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2)
        {
          var propertyName = parts[0].Trim();
          var errorMessageText = parts[1].Trim();

          if (!errors.ContainsKey(propertyName))
          {
            errors[propertyName] = [];
          }

          // Remove "Severity: Error" from the message if present
          errors[propertyName].Add(errorMessageText.Replace("Severity: Error", "").Trim());
        }
      }
    }

    return errors;
  }
}