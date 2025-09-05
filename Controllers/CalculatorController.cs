using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Domain;
using MyBlazorServerApp.Infrastructure.PostgreSQL;
using MyBlazorServerApp.Resources;



[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly PostgreSQLRepository _repository;
  private readonly IMapper _mapper;

  public CalculatorController(IMediator mediator, PostgreSQLRepository repository, IMapper mapper)
  {
    _mediator = mediator;
    _repository = repository;
    _mapper = mapper;
  }

  /// <summary>
  /// Calculates the monthly installment amount using a command.
  /// </summary>
  /// <param name="installment">Calculate installment request containing the total amount and number of installments.</param>
  /// <returns>The calculated monthly installment.</returns>
  [HttpPost("calculate-installment")]
  public async Task<ActionResult<Installment>> CalculateInstallment([FromBody] Installment installment)
  {
    return await _mediator.Send(
      new CalculateInstallmentCommand { Amount = installment.Amount, NumberOfInstallments = installment.NumberOfInstallments });
  }

  /// <summary>
  /// Calculates the number of installments (duration) using a command.
  /// </summary>
  /// <param name="request">Calculate duration request containing the total amount and monthly installment amount.</param>
  /// <returns>The calculated number of installments.</returns>
  [HttpPost("calculate-duration")]
  public async Task<ActionResult<Duration>> CalculateDuration([FromBody] Duration duration)
  {
    return await _mediator.Send(
      new CalculateDurationCommand { Amount = duration.Amount, MonthlyInstallment = duration.MonthlyInstallment });
  }

  /// <summary>
  /// Saves a calculation of type Installment to the database.
  /// </summary>
  /// <param name="calculation">The installment calculation details to be saved.</param>
  /// <returns>An action result indicating success.</returns>
  [HttpPost("save-installment")]
  public async Task<IActionResult> SaveInstallment([FromBody] Installment calculation)
  {
    var newCalculation = _mapper.Map<InstallmentEntity>(calculation);
    _repository.Installments.Add(newCalculation);
    await _repository.SaveChangesAsync();

    return Ok();
  }



  /// <summary>
  /// Saves a calculation of type Duration to the database.
  /// </summary>
  /// <param name="calculation">The duration calculation details to be saved.</param>
  /// <returns>An action result indicating success.</returns>
  [HttpPost("save-duration")]
  public async Task<IActionResult> SaveDuration([FromBody] Duration calculation)
  {
    var newCalculation = _mapper.Map<DurationEntity>(calculation);
    _repository.Durations.Add(newCalculation);
    await _repository.SaveChangesAsync();

    return Ok();
  }


  /// <summary>
  /// Gets all saved installment calculations from the database.
  /// </summary>
  /// <returns>A list of saved installment calculations.</returns>
  [HttpGet("get-all-installments")]
  public async Task<ActionResult<List<Installment>>> GetAllInstallments()
  {
    var installments = await _repository.Installments.ToListAsync();
    return _mapper.Map<List<Installment>>(installments);
  }

  /// <summary>
  /// Gets all saved duration calculations from the database.
  /// </summary>
  /// <returns>A list of saved duration calculations.</returns>
  [HttpGet("get-all-durations")]
  public async Task<ActionResult<List<Duration>>> GetAllDurations()
  {
    var durations = await _repository.Durations.ToListAsync();
    return _mapper.Map<List<Duration>>(durations);
  }
}
