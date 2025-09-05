using FluentValidation;
using MyBlazorServerApp.Commands;

/// <summary>
/// Validator for the CalculateInstallmentCommand.
/// Ensures that the command parameters meet the required business rules before processing.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculateInstallmentCommandValidator"/> class.
/// This validator is automatically used by the ValidationBehavior middleware to validate
/// CalculateInstallmentCommand instances before they are handled.
/// </remarks>
public class CalculateInstallmentCommandValidator : AbstractValidator<CalculateInstallmentCommand>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CalculateInstallmentCommandValidator"/> class.
  /// Configures the validation rules for the CalculateInstallmentCommand.
  /// </summary>
  public CalculateInstallmentCommandValidator()
  {
    RuleFor(x => x.Amount)
      .GreaterThan(0).WithMessage("Amount must be greater than zero.");

    RuleFor(x => x.NumberOfInstallments)
      .GreaterThan(0).WithMessage("Number of installments must be greater than zero.");
  }
}