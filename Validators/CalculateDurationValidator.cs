using FluentValidation;
using MyBlazorServerApp.Commands;

/// <summary>
/// Validator for the CalculateInstallmentCommand.
/// </summary>
public class CalculateInstallmentCommandValidator : AbstractValidator<CalculateInstallmentCommand>
{
  public CalculateInstallmentCommandValidator()
  {
    RuleFor(x => x.Amount)
      .GreaterThan(0).WithMessage("Amount must be greater than zero.");

    RuleFor(x => x.NumberOfInstallments)
      .GreaterThan(0).WithMessage("Number of installments must be greater than zero.");
  }
}