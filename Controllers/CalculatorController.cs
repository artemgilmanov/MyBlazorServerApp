using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlazorServerApp.Commands;
using MyBlazorServerApp.Domain;
using MyBlazorServerApp.Infrastructure.PostgreSQL;
using MyBlazorServerApp.Resources;
using System.Threading;

/// <summary>
/// API controller for financial calculation operations including installment and duration calculations.
/// Provides endpoints for calculating, saving, and retrieving financial calculations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly PostgreSQLRepository _repository;
  private readonly IMapper _mapper;

  /// <summary>
  /// Initializes a new instance of the <see cref="CalculatorController"/> class.
  /// </summary>
  /// <param name="mediator">The mediator for handling CQRS commands and queries.</param>
  /// <param name="repository">The repository for database operations.</param>
  /// <param name="mapper">The AutoMapper instance for object mapping.</param>
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
  /// <response code="200">Returns the calculated monthly installment amount.</response>
  /// <response code="400">If the request parameters are invalid.</response>
  [HttpPost("calculate-installment")]
  [ProducesResponseType(typeof(Installment), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<Installment>> CalculateInstallment([FromBody] Installment installment)
  {
    return await _mediator.Send(
      new CalculateInstallmentCommand { Amount = installment.Amount, NumberOfInstallments = installment.NumberOfInstallments });
  }

  /// <summary>
  /// Calculates the number of installments (duration) using a command.
  /// </summary>
  /// <param name="duration">Calculate duration request containing the total amount and monthly installment amount.</param>
  /// <returns>The calculated number of installments.</returns>
  /// <response code="200">Returns the calculated number of installments.</response>
  /// <response code="400">If the request parameters are invalid.</response>
  [HttpPost("calculate-duration")]
  [ProducesResponseType(typeof(Duration), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
  /// <response code="200">The calculation was successfully saved.</response>
  /// <response code="400">If the calculation data is invalid.</response>
  /// <response code="500">If there was an error saving to the database.</response>
  [HttpPost("save-installment")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
  /// <response code="200">The calculation was successfully saved.</response>
  /// <response code="400">If the calculation data is invalid.</response>
  /// <response code="500">If there was an error saving to the database.</response>
  [HttpPost("save-duration")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
  /// <response code="200">Returns the list of saved duration calculations.</response>
  [HttpGet("get-all-durations")]
  [ProducesResponseType(typeof(List<Duration>), StatusCodes.Status200OK)]
  public async Task<ActionResult<List<Duration>>> GetAllDurations()
  {
    var durations = await _repository.Durations.ToListAsync();
    return _mapper.Map<List<Duration>>(durations);
  }
}
