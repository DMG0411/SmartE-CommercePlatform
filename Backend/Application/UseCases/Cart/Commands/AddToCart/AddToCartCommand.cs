using MediatR;
using System;

namespace Application.UseCases.Cart.Commands.AddToCart
{
    public class AddToCartCommand : IRequest<Unit>
    {
        public Guid UserId { get; }
        public Guid ProductId { get; }

        public AddToCartCommand(Guid userId, Guid productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }
}