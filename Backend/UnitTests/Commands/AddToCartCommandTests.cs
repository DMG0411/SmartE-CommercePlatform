using Application.UseCases.Cart.Commands.AddToCart;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace UnitTests.Commands
{
    public class AddToCartCommandTests
    {
        private readonly ICartRepository cartRepository;

        public AddToCartCommandTests()
        {
            cartRepository = Substitute.For<ICartRepository>();
        }

        [Fact]
        public async void AddToCartCommand_ValidCommand_ShouldPass()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var command = new AddToCartCommand(userId, productId);
            var handler = new AddToCartCommandHandler(cartRepository);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            await cartRepository.Received(1).AddToCart(userId, productId);
        }

        //[Fact]
        //public async void AddToCartCommand_InvalidCommand_ShouldFail()
        //{
        //    // Arrange
        //    var userId = Guid.Empty;
        //    var productId = Guid.Empty;
        //    var command = new AddToCartCommand(userId, productId);
        //    var handler = new AddToCartCommandHandler(cartRepository);

        //    // Act & Assert
        //    await handler.Handle(command, CancellationToken.None);
        //    await cartRepository.Received(0).AddToCart(userId, productId); 
        //}
    }
}
