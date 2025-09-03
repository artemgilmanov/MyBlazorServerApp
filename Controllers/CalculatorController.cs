using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Resources;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
  private readonly IMediator _mediator;

  public CalculatorController(IMediator mediator)
  {
    _mediator = mediator;
  }

  /// <summary>
  /// Calculates the monthly installment amount using a command.
  /// </summary>
  /// <param name="request">Calculate installment request containing the total amount and number of installments.</param>
  /// <returns>The calculated monthly installment.</returns>
  [HttpPost("calculate-installment")]
  public async Task<ActionResult<CalculationResult>> CalculateInstallment([FromBody] CalculateInstallmentRequest request)
  {
    return await _mediator.Send(
      new CalculateInstallmentCommand { Amount = request.Amount, NumberOfInstallments = request.NumberOfInstallments } );
  }

  /// <summary>
  /// Calculates the number of installments (duration) using a command.
  /// </summary>
  /// <param name="request">Calculate duration request containing the total amount and monthly installment amount.</param>
  /// <returns>The calculated number of installments.</returns>
  [HttpPost("calculate-duration")]
  public async Task<ActionResult<CalculationResult>> CalculateDuration([FromBody] CalculateDurationRequest request)
  {
    return await _mediator.Send(
      new CalculateDurationCommand { Amount = request.Amount, MonthlyInstallment = request.MonthlyInstallment });
  }
}
