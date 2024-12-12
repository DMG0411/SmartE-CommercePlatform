using FluentValidation;
using MediatR;
using Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Cart.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Unit>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IValidator<AddToCartCommand> _validator;

        public AddToCartCommandHandler(ICartRepository cartRepository, IValidator<AddToCartCommand> validator)
        {
            _cartRepository = cartRepository;
            _validator = validator;
        }

        public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Unit.Value;
            }

            await _cartRepository.AddToCart(request.UserId, request.ProductId);

            return Unit.Value;
        }
    }
}
