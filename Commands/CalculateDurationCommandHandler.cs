using MediatR;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Infrastructure.CalculationRepository;
using MyBlazorServerApp.Resources;

public class CalculateDurationCommandHandler : IRequestHandler<CalculateDurationCommand, Duration>
{
  private readonly ICalculationRepository _calculationRepository;

  public CalculateDurationCommandHandler(ICalculationRepository calculationRepository)
  {
    _calculationRepository = calculationRepository;
  }

  public Task<Duration> Handle(CalculateDurationCommand request, CancellationToken cancellationToken)
  {
    var numberOfInstallments = _calculationRepository.CalculateDuration(request.Amount, request.MonthlyInstallment);
    return Task.FromResult(new Duration { Result = numberOfInstallments });
  }
}
