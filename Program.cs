// This file configures the application services and request pipeline.

using MediatR;
using FluentValidation;
using MyBlazorServerApp.Components;
using MyBlazorServerApp.Infrastructure;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

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
