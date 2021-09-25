using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
  public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
  {
    public UpdateOrderCommandValidator()
    {
      RuleFor(x => x.UserName).NotEmpty().WithMessage("{Username} is required.").NotNull().MaximumLength(50)
        .WithMessage("{Username} must not exceed 50 characters.");

      RuleFor(x => x.EmailAddress)
        .NotEmpty().WithMessage("{EmailAddress} is required.")
        .EmailAddress().WithMessage("{EmailAddress} is invalid. Please enter a valid email address.");

      RuleFor(x => x.TotalPrice)
        .NotEmpty().WithMessage("{TotalPrice} is required.")
        .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");
    }
  }
}