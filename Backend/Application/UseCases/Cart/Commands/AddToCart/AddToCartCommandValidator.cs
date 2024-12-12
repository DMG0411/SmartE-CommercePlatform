using FluentValidation;
using Application.UseCases.Cart.Commands.AddToCart;

public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("User ID must not be empty.");

        RuleFor(command => command.ProductId)
            .NotEmpty().WithMessage("Product ID must not be empty.");
    }
}
