// This file configures the application services and request pipeline.

using MediatR;
using MyBlazorServerApp.Components;
using MyBlazorServerApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure MediatR to find commands and handlers in the current assembly.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register the ICalculationRepository as a scoped service.
builder.Services.AddScoped<ICalculationRepository, CalculationRepository>();

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
