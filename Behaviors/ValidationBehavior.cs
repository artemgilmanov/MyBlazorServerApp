using FluentValidation;
using MediatR;

/// <summary>
/// A MediatR pipeline behavior that validates incoming commands before they are handled.
/// </summary>
/// <typeparam name="TRequest">The type of the request (command).</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationBehavior<TRequest, TResponse>(
  IValidator<TRequest>? validator, 
  ILogger<ValidationBehavior<TRequest, TResponse>> logger) : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
  private readonly IValidator<TRequest>? _validator = validator;
  private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger = logger;

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Request {RequestType} started to execute.", typeof(TRequest).Name);

    await _validator.ValidateAndThrowAsync(request, cancellationToken);

    var response = await next(cancellationToken);

    _logger.LogInformation("Request {RequestType} executed successfully.", typeof(TRequest).Name);

    return response;

  }
}
