using MyBlazorServerApp.Middleware;

/// <summary>
/// Global exception handling middleware for the application.
/// This middleware catches all unhandled exceptions that occur during request processing
/// and provides a consistent error response format.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
/// This middleware should be registered early in the request pipeline to ensure
/// all subsequent middleware exceptions are caught.
/// </remarks>
/// <param name="next">The next middleware in the request pipeline.</param>
public class ExceptionMiddleware(RequestDelegate next)
{
  /// <summary>
  /// The next middleware delegate in the pipeline.
  /// </summary>
  private readonly RequestDelegate _next = next;

  /// <summary>
  /// Processes an HTTP request and catches any unhandled exceptions that occur
  /// during the execution of subsequent middleware components.
  /// </summary>
  /// <param name="context">The HTTP context for the current request.</param>
  /// <returns>A task that represents the asynchronous exception handling operation.</returns>
  /// <remarks>
  /// If an exception occurs during request processing, this method delegates
  /// the exception handling to the <see cref="ExceptionMiddlewareHelper.HandleExceptionAsync"/>
  /// method to generate a consistent error response.
  /// </remarks>
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await ExceptionMiddlewareHelper.HandleExceptionAsync(context, ex);
    }
  }
}
