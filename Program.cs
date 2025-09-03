using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using MyBlazorServerApp.Components;
using MyBlazorServerApp.Infrastructure;
using Serilog;
using System.Net;

var logsDirectory = "Logs";
if (!Directory.Exists(logsDirectory))
{
  Directory.CreateDirectory(logsDirectory);
}

var configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .AddJsonFile("appsettings.Development.json", optional: true)
  .AddJsonFile("serilog.json")
  .AddEnvironmentVariables()
  .Build();

// Configure Serilog to read from the built configuration.
Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(configuration)
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure MediatR to find commands and handlers in the current assembly.
builder.Services.AddMediatR(cfg =>
{
  cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
  cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

// Register the ICalculationRepository as a scoped service.
builder.Services.AddScoped<ICalculationRepository, CalculationRepository>();

// Register Validation.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddHttpClient("MyHttpClient", client =>
{
  client.BaseAddress = new Uri("http://localhost:5137");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  app.UseHsts();
}

app.UseExceptionHandler(exceptionHandlerApp =>
{
  exceptionHandlerApp.Run(async context =>
  {
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature?.Error;

    if (exception is ValidationException validationException)
    {
      Log.Warning("Validation failed for request: {RequestPath} with errors: {@Errors}",
                    context.Request.Path,
                    validationException.Errors);

      context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
      context.Response.ContentType = "application/json";

      var errors = new Dictionary<string, List<string>>();
      foreach (var error in validationException.Errors)
      {
        if (!errors.ContainsKey(error.PropertyName))
        {
          errors[error.PropertyName] = new List<string>();
        }
        errors[error.PropertyName].Add(error.ErrorMessage);
      }

      await context.Response.WriteAsJsonAsync(new { errors = errors });
    }
  });
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
