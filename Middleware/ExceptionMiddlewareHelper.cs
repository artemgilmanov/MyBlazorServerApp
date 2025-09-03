using MyBlazorServerApp.Exceptions;
using Serilog;
using System.Net;
using System.Text.Json;

namespace MyBlazorServerApp.Middleware;

internal static class ExceptionMiddlewareHelper
{
  internal static Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    if (exception is ValidationException validationException)
    {
      Log.Warning("Validation failed for request: {RequestPath} with errors: {@Errors}", context.Request.Path, validationException.Errors);

      context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      context.Response.ContentType = "application/json";

      return context.Response.WriteAsJsonAsync(new { errors = validationException.Errors });
    }
    // Handle unhandled exceptions, including those with validation messages in the string.
    else if (exception.Message.StartsWith("Validation failed:"))
    {
      Log.Warning("Validation failed for request: {RequestPath} with message: {Message}", context.Request.Path, exception.Message);

      context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      context.Response.ContentType = "application/json";

      var errors = new Dictionary<string, List<string>>();
      var lines = exception.Message.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);
      foreach (var line in lines)
      {
        if (line.StartsWith(" -- "))
        {
          var parts = line.Substring(4).Split([": "], 2, StringSplitOptions.RemoveEmptyEntries);
          if (parts.Length == 2)
          {
            var propertyName = parts[0].Trim();
            var errorMessage = parts[1].Trim();
            if (!errors.ContainsKey(propertyName))
            {
              errors[propertyName] = [];
            }
            errors[propertyName].Add(errorMessage.Replace("Severity: Error", "").Trim());
          }
        }
      }
      return context.Response.WriteAsJsonAsync(new { errors });
    }
    else
    {
      Log.Error(exception, "An unhandled exception occurred during request: {RequestPath}", context.Request.Path);
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      context.Response.ContentType = "application/json";
      var result = JsonSerializer.Serialize(new { error = "An internal server error has occurred.", details = exception.Message });
      return context.Response.WriteAsync(result);
    }
  }
}
