using FluentValidation;

namespace Application.UseCases.Cart.Queries.GetCartById
{
    public class GetCartByIdQueryValidator : AbstractValidator<GetCartByIdQuery>
    {
        public GetCartByIdQueryValidator()
        {
            RuleFor(query => query.UserId)
                .NotEmpty().WithMessage("UserId must be provided.")
                .Must(id => id != Guid.Empty).WithMessage("UserId cannot be empty.");
        }
    }
}
