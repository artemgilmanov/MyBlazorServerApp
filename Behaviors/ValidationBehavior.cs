using FluentValidation;
using MediatR;

/// <summary>
/// A MediatR pipeline behavior that validates incoming commands before they are handled.
/// This behavior ensures that all requests are validated using FluentValidation
/// before being processed by their respective handlers.
/// </summary>
/// <typeparam name="TRequest">The type of the request (command).</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}" /> class.
/// </remarks>
/// <param name="validator">The FluentValidation validator for the request type.</param>
/// <param name="logger">The logger for tracking validation behavior.</param>
public class ValidationBehavior<TRequest, TResponse>(
  IValidator<TRequest>? validator, 
  ILogger<ValidationBehavior<TRequest, TResponse>> logger) : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
  /// <summary>
  /// The FluentValidation validator instance for the request type.
  /// </summary>
  private readonly IValidator<TRequest>? _validator = validator;

  /// <summary>
  /// The FluentValidation validator instance for the request type.
  /// </summary>
  private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger = logger;

  /// <summary>
  /// Handles the request pipeline by validating the incoming request before passing it to the next handler.
  /// If validation fails, a ValidationException is thrown preventing the request from being processed.
  /// </summary>
  /// <param name="request">The incoming request to validate and process.</param>
  /// <param name="next">The delegate to invoke the next handler in the pipeline.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>
  /// The response from the next handler in the pipeline if validation succeeds.
  /// </returns>
  /// <exception cref="ValidationException">
  /// Thrown when the request fails validation according to the configured validation rules.
  /// </exception>
  public async Task<TResponse> Handle(
    TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Request {RequestType} started to execute.", typeof(TRequest).Name);

    await _validator.ValidateAndThrowAsync(request, cancellationToken);

    // Proceed to the next handler in the pipeline
    var response = await next(cancellationToken);

    _logger.LogInformation("Request {RequestType} executed successfully.", typeof(TRequest).Name);

    return response;
  }
}
