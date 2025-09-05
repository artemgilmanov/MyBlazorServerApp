using FluentValidation;

using MyBlazorServerApp.Commands;

/// <summary>
/// Validator for the CalculateDurationCommand.
/// Ensures that the command parameters meet the required business rules before processing.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculateDurationCommandValidator"/> class.
/// This validator is automatically used by the ValidationBehavior middleware to validate
/// CalculateDurationCommand instances before they are handled by the command handler.
/// </remarks>
public class CalculateDurationCommandValidator : AbstractValidator<CalculateDurationCommand>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CalculateDurationCommandValidator"/> class.
  /// Configures the validation rules for the CalculateDurationCommand parameters.
  /// </summary>
  public CalculateDurationCommandValidator()
  {
    RuleFor(x => x.Amount)
      .GreaterThan(0).WithMessage("Amount must be greater than zero.");

    RuleFor(x => x.MonthlyInstallment)
      .GreaterThan(0).WithMessage("Monthly installment must be greater than zero.");
  }
}
