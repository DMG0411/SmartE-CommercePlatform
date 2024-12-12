using FluentValidation;

namespace Application.UseCases.Cart.Commands.RemoveFromCart
{
    public class RemoveFromCartCommandValidator : AbstractValidator<RemoveFromCartCommand>
    {
        public RemoveFromCartCommandValidator()
        {
            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("UserId must not be empty.");

            RuleFor(command => command.ProductId)
                .NotEmpty().WithMessage("ProductId must not be empty.");
        }
    }
}
