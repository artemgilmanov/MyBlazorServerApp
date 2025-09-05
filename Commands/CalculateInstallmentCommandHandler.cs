using MediatR;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Infrastructure.CalculationRepository;
using MyBlazorServerApp.Resources;

/// <summary>
/// The handler for the CalculateInstallmentCommand.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculateInstallmentCommandHandler" /> class.
/// </remarks>
/// <param name="calculationRepository">The calculation repository used for installment calculations.</param>
public class CalculateInstallmentCommandHandler : IRequestHandler<CalculateInstallmentCommand, Installment>
{
  /// <summary>
  /// The calculation repository.
  /// </summary>
  private readonly ICalculationRepository _calculationRepository;

  /// <summary>
  /// The calculation repository.
  /// </summary>
  public CalculateInstallmentCommandHandler(ICalculationRepository calculationRepository)
  {
    _calculationRepository = calculationRepository;
  }

  /// <summary>
  /// Handles the installment calculation request by processing the amount and number of installments
  /// to determine the monthly installment amount.
  /// </summary>
  /// <param name="request">The installment calculation command containing amount and number of installments.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A <see cref="Task{Installment}"/> containing the calculated installment result.
  /// </returns>
  public Task<Installment> Handle(CalculateInstallmentCommand request, CancellationToken cancellationToken)
  {
    var monthlyInstallment = _calculationRepository.CalculateInstallment(request.Amount, request.NumberOfInstallments);
    return Task.FromResult(new Installment { Amount = request.Amount, NumberOfInstallments = request.NumberOfInstallments,  Result = monthlyInstallment });
  }
}