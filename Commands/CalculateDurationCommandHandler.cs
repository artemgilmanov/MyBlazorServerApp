using MediatR;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Infrastructure.CalculationRepository;
using MyBlazorServerApp.Resources;

/// <summary>
/// The handler for the CalculateDurationCommand.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculateDurationCommandHandler" /> class.
/// </remarks>
/// <param name="calculationRepository">The calculation repository used for duration calculations.</param>
public class CalculateDurationCommandHandler : IRequestHandler<CalculateDurationCommand, Duration>
{
  /// <summary>
  /// The calculation repository.
  /// </summary>
  private readonly ICalculationRepository _calculationRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="CalculateDurationCommandHandler" /> class.
  /// </summary>
  /// <param name="calculationRepository">The calculation repository used for duration calculations.</param>
  public CalculateDurationCommandHandler(ICalculationRepository calculationRepository)
  {
    _calculationRepository = calculationRepository;
  }

  /// <summary>
  /// Handles the duration calculation request by processing the amount and monthly installment
  /// to determine the number of installments required.
  /// </summary>
  /// <param name="request">The duration calculation command containing amount and monthly installment.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// A <see cref="Task{Duration}"/> containing the calculated duration result.
  /// </returns>
  public Task<Duration> Handle(CalculateDurationCommand request, CancellationToken cancellationToken)
  {
    var numberOfInstallments = _calculationRepository.CalculateDuration(request.Amount, request.MonthlyInstallment);
    return Task.FromResult(new Duration { Amount = request.Amount, MonthlyInstallment=request.MonthlyInstallment, Result = numberOfInstallments });
  }
}
