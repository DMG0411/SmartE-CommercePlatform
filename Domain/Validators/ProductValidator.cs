using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator() {
            RuleFor(product => product.Type)
                .NotEmpty().WithMessage("Type is required")
                .MaximumLength(50).WithMessage("Type must be less than 50 characters long");

            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must be less than 100 characters long");

            RuleFor(product => product.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long");

            RuleFor(product => product.Price)
                .NotEmpty()
                .WithMessage("Price is required");

            RuleFor(product => product.Review)
                .NotEmpty().WithMessage("Review is required")
                .Must(isAValidReview).WithMessage("Review is not valid");
        }

        private bool isAValidReview(int review)
        {
            return review >= 0 && review <= 5;
        }
    }
}
