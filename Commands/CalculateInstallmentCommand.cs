using MediatR;
using MyBlazorServerApp.Resources;

namespace MyBlazorServerApp.Commands;

public class CalculateInstallmentCommand : IRequest<CalculationResult>
{
  public double Amount { get; set; }
  public int NumberOfInstallments { get; set; }
}