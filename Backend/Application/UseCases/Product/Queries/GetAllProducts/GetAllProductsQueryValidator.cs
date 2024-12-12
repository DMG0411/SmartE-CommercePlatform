using FluentValidation;

namespace Application.UseCases.Product.Queries.GetAllProducts
{
    public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
    {
        public GetAllProductsQueryValidator()
        {
            RuleFor(query => query.PageNumber)
                .GreaterThanOrEqualTo(0).WithMessage("PageNumber must be non-negative.");

            RuleFor(query => query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than zero.")
                .LessThanOrEqualTo(100).WithMessage("PageSize must not exceed 100.");

            When(query => query.MinPrice.HasValue && query.MaxPrice.HasValue, () =>
            {
                RuleFor(query => query)
                    .Must(q => q.MinPrice <= q.MaxPrice)
                    .WithMessage("MinPrice must be less than or equal to MaxPrice.");
            });

            RuleFor(query => query.MinReview)
                .InclusiveBetween(0, 5).WithMessage("MinReview must be between 0 and 5.");
        }
    }
}
