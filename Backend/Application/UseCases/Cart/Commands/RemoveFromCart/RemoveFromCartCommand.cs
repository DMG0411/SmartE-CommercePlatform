using MediatR;

namespace Application.UseCases.Cart.Commands.RemoveFromCart
{
    public class RemoveFromCartCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public RemoveFromCartCommand(Guid userId, Guid productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }
}
