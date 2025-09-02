using Microsoft.AspNetCore.Mvc;
using MyBlazorServerApp.Infrastructure;
using MyBlazorServerApp.Resources;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
  private readonly ICalculationRepository _calculationRepository;

  public CalculatorController(ICalculationRepository calculationRepository)
  {
    _calculationRepository = calculationRepository;
  }

  /// <summary>
  /// Calculates the monthly installment amount.
  /// </summary>
  /// <param name="request">The request containing the total amount and number of installments.</param>
  /// <returns>The calculated monthly installment.</returns>
  [HttpPost("calculate-installment")]
  public ActionResult<CalculationResult> CalculateInstallment([FromBody] CalculateInstallmentRequest request)
  {
    if (request.Amount <= 0 || request.NumberOfInstallments <= 0)
    {
      return BadRequest("Amount and number of installments must be greater than zero.");
    }

    var monthlyInstallment = _calculationRepository.CalculateInstallment(request.Amount, request.NumberOfInstallments);
    return Ok(new CalculationResult { Result = monthlyInstallment });
  }

  /// <summary>
  /// Calculates the number of installments (duration).
  /// </summary>
  /// <param name="request">The request containing the total amount and monthly installment amount.</param>
  /// <returns>The calculated number of installments.</returns>
  [HttpPost("calculate-duration")]
  public ActionResult<CalculationResult> CalculateDuration([FromBody] CalculateDurationRequest request)
  {
    if (request.Amount <= 0 || request.MonthlyInstallment <= 0)
    {
      return BadRequest("Amount and monthly installment must be greater than zero.");
    }

    var numberOfInstallments = _calculationRepository.CalculateDuration(request.Amount, request.MonthlyInstallment);
    return Ok(new CalculationResult { Result = numberOfInstallments });
  }
}
