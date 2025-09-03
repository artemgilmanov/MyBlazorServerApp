using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBlazorServerApp.Components;
using MyBlazorServerApp.Infrastructure;
using Serilog;
using System.Net;
using System.Reflection;

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

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<Repository>(options =>
//    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<Repository>(options =>
    options.UseInMemoryDatabase("MyBlazorServerAppDB"));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


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

// Register ExceptionMiddleware.
app.UseMiddleware<ExceptionMiddleware>();

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
