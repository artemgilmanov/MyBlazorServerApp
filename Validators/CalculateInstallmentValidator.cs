using FluentValidation;

using MyBlazorServerApp.Commands;

/// <summary>
/// Validator for the CalculateDurationCommand.
/// </summary>
public class CalculateDurationCommandValidator : AbstractValidator<CalculateDurationCommand>
{
  public CalculateDurationCommandValidator()
  {
    RuleFor(x => x.Amount)
        .GreaterThan(0).WithMessage("Amount must be greater than zero.");

    RuleFor(x => x.MonthlyInstallment)
        .GreaterThan(0).WithMessage("Monthly installment must be greater than zero.");
  }
}
