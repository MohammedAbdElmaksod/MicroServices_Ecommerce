
using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
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

        RuleFor(o => o.FirstName)
            .NotEmpty()
            .NotNull()
            .WithMessage("FirstName is Required");
        
        RuleFor(o => o.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("LastName is Required");
     
        RuleFor(o => o.EmailAddress)
            .NotEmpty()
            .NotNull()
            .WithMessage("EmailAddress is Required");
    }
}
