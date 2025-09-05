using MediatR;
using MyBlazorServerApp.Resources;

namespace MyBlazorServerApp.Commands;

/// <summary>
/// The command to calculate the duration based on amount and monthly installment.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculateDurationCommand"/> class.
/// </remarks>
public class CalculateDurationCommand : IRequest<Duration>
{
  /// <summary>
  /// Gets or sets the total amount for which the duration is being calculated.
  /// </summary>
  public double Amount { get; set; }

  /// <summary>
  /// Gets or sets the monthly installment amount.
  /// </summary>
  public double MonthlyInstallment { get; set; }
}
