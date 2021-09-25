using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
  public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
  {
    public DeleteOrderCommandValidator()
    {
      RuleFor(x => x.Id).NotEmpty().WithMessage("{Id} is required.");
    }
  }
}