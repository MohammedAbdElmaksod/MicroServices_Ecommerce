using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class CheckoutOrderCommandValidatorV2 : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidatorV2()
    {
        RuleFor(o => o.UserName)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserName is Required")
            .MaximumLength(70)
            .WithMessage("UserName must not excced 70 characters");

        RuleFor(o => o.TotalPrice)
           .NotEmpty()
           .NotNull()
           .WithMessage("TotalPrice is Required")
           .GreaterThan(-1)
           .WithMessage("Total price must not be negative");
    }
}