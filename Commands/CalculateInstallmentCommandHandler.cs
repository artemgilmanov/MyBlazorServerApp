using MediatR;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Infrastructure;
using MyBlazorServerApp.Resources;

public class CalculateInstallmentCommandHandler : IRequestHandler<CalculateInstallmentCommand, CalculationResult>
{
  private readonly ICalculationRepository _calculationRepository;

  public CalculateInstallmentCommandHandler(ICalculationRepository calculationRepository)
  {
    _calculationRepository = calculationRepository;
  }

  public Task<CalculationResult> Handle(CalculateInstallmentCommand request, CancellationToken cancellationToken)
  {
    var monthlyInstallment = _calculationRepository.CalculateInstallment(request.Amount, request.NumberOfInstallments);
    return Task.FromResult(new CalculationResult { Result = monthlyInstallment });
  }
}