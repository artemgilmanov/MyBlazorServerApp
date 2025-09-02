using MediatR;
using MyBlazorServerApp.Resources;

namespace MyBlazorServerApp.Commands;

public class CalculateDurationCommand : IRequest<CalculationResult>
{
  public double Amount { get; set; }
  public double MonthlyInstallment { get; set; }
}
