using FluentValidation;
using MediatR;
using Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Cart.Commands.RemoveFromCart
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, Unit>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IValidator<RemoveFromCartCommand> _validator;

        public RemoveFromCartCommandHandler(ICartRepository cartRepository, IValidator<RemoveFromCartCommand> validator)
        {
            _cartRepository = cartRepository;
            _validator = validator;
        }

        public async Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Invalid RemoveFromCartCommand: {errors}");
            }

            await _cartRepository.RemoveFromCart(request.UserId, request.ProductId);

            return Unit.Value;
        }
    }
}
