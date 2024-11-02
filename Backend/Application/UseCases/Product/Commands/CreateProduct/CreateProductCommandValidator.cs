using FluentValidation;

namespace Application.UseCases.Product.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Product.Type)
                .NotEmpty().WithMessage("Type is required")
                .MaximumLength(50).WithMessage("Type must be less than 50 characters long");

            RuleFor(product => product.Product.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must be less than 100 characters long");

            RuleFor(product => product.Product.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long");

            RuleFor(product => product.Product.Price)
                .NotEmpty()
                .WithMessage("Price is required");

            RuleFor(product => product.Product.Review)
                .NotEmpty().WithMessage("Review is required")
                .Must(isAValidReview).WithMessage("Review is not valid");
        }

        private bool isAValidReview(int review)
        {
            return review >= 0 && review <= 5;
        }
    }
}
