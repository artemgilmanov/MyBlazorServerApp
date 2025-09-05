using MediatR;
using MyBlazorServerApp.Resources;

namespace MyBlazorServerApp.Commands;

/// <summary>
/// The command to calculate the installment amount based on total amount and number of installments.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculateInstallmentCommand"/> class.
/// </remarks>
public class CalculateInstallmentCommand : IRequest<Installment>
{
  /// <summary>
  /// Gets or sets the total amount for which installments are being calculated.
  /// </summary>
  public double Amount { get; set; }

  /// <summary>
  /// Gets or sets the total amount for which installments are being calculated.
  /// </summary>
  public int NumberOfInstallments { get; set; }
}