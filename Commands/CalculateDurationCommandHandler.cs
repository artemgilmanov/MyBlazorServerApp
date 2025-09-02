using MediatR;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Infrastructure;
using MyBlazorServerApp.Resources;

public class CalculateDurationCommandHandler : IRequestHandler<CalculateDurationCommand, CalculationResult>
{
  private readonly ICalculationRepository _calculationRepository;

  public CalculateDurationCommandHandler(ICalculationRepository calculationRepository)
  {
    _calculationRepository = calculationRepository;
  }

  public Task<CalculationResult> Handle(CalculateDurationCommand request, CancellationToken cancellationToken)
  {
    var numberOfInstallments = _calculationRepository.CalculateDuration(request.Amount, request.MonthlyInstallment);
    return Task.FromResult(new CalculationResult { Result = numberOfInstallments });
  }
}
