using MyBlazorServerApp.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;

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
