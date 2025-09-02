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
    if (request.Amount <= 0 || request.NumberOfInstallments <= 0)
    {
      throw new ArgumentException("Amount and number of installments must be greater than zero.");
    }

    var monthlyInstallment = _calculationRepository.CalculateInstallment(request.Amount, request.NumberOfInstallments);
    return Task.FromResult(new CalculationResult { Result = monthlyInstallment });
  }
}