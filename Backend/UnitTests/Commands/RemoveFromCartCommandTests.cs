using Application.UseCases.Cart.Commands.RemoveFromCart;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace UnitTests.Commands
{
    public class RemoveFromCartCommandTests
    {
        private readonly ICartRepository cartRepository;

        public RemoveFromCartCommandTests()
        {
            cartRepository = Substitute.For<ICartRepository>();
        }

        [Fact]
        public async void RemoveFromCartCommand_ValidCommand_ShouldPass()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var command = new RemoveFromCartCommand(userId, productId);
            var handler = new RemoveFromCartCommandHandler(cartRepository);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            await cartRepository.Received(1).RemoveFromCart(userId, productId);
        }

        //[Fact]
        //public async void RemoveFromCartCommand_InvalidCommand_ShouldFail()
        //{
        //    // Arrange
        //    var userId = Guid.Empty;
        //    var productId = Guid.Empty;
        //    var command = new RemoveFromCartCommand(userId, productId);
        //    var handler = new RemoveFromCartCommandHandler(cartRepository);

        //    // Act & Assert
        //    await handler.Handle(command, CancellationToken.None);
        //    await cartRepository.Received(0).RemoveFromCart(userId, productId); 
        //}
    }
}
