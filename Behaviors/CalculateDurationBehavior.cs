// This file defines a MediatR pipeline behavior that uses Fluent Validation.

using FluentValidation;
using MediatR;

/// <summary>
/// A MediatR pipeline behavior that validates incoming commands before they are handled.
/// </summary>
/// <typeparam name="TRequest">The type of the request (command).</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
  private readonly IValidator<TRequest>? _validator;

  public ValidationBehavior(IValidator<TRequest>? validator = null)
  {
    _validator = validator;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    // If a validator exists for this request type, execute it.
    if (_validator != null)
    {
      // The method is now called directly on the request, which resolves the compilation error.
      await _validator.ValidateAndThrowAsync(request, cancellationToken);
    }

    // Proceed to the next step in the pipeline (which will be the command handler).
    return await next();
  }
}
