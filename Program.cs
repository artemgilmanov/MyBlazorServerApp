using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBlazorServerApp.Components;
using MyBlazorServerApp.Infrastructure.CalculationRepository;
using MyBlazorServerApp.Infrastructure.PostgreSQL;
using Serilog;
using System.Reflection;

var logsDirectory = "Logs";
if (!Directory.Exists(logsDirectory))
{
  Directory.CreateDirectory(logsDirectory);
}

# region "Configure Serilog"
var configuration = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .AddJsonFile("appsettings.Development.json", optional: true)
  .AddJsonFile("serilog.json")
  .AddEnvironmentVariables()
  .Build();

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(configuration)
  .CreateLogger();
# endregion

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

// Register Validation.
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

# region "Configure Repositories"
// Register PostgreSQLRepository
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<PostgreSQLRepository>(options =>
//  options.UseNpgsql(connectionString));

// Register UseInMemoryDatabase
builder.Services.AddDbContext<PostgreSQLRepository>(options =>
  options.UseInMemoryDatabase("MyBlazorServerAppDB"));

// Register the ICalculationRepository.
builder.Services.AddScoped<ICalculationRepository, CalculationRepository>();
# endregion

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
