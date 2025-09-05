using MediatR;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Infrastructure.CalculationRepository;
using MyBlazorServerApp.Resources;

public class CalculateInstallmentCommandHandler : IRequestHandler<CalculateInstallmentCommand, Installment>
{
  private readonly ICalculationRepository _calculationRepository;

  public CalculateInstallmentCommandHandler(ICalculationRepository calculationRepository)
  {
    _calculationRepository = calculationRepository;
  }

  public Task<Installment> Handle(CalculateInstallmentCommand request, CancellationToken cancellationToken)
  {
    var monthlyInstallment = _calculationRepository.CalculateInstallment(request.Amount, request.NumberOfInstallments);
    return Task.FromResult(new Installment { Result = monthlyInstallment });
  }
}