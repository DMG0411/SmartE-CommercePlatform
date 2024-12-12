using Application.UseCases.Cart.Commands.RemoveFromCart;
using Domain.Repositories;
using NSubstitute;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace UnitTests.Commands
{
    public class RemoveFromCartCommandHandlerTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IValidator<RemoveFromCartCommand> _validator;
        private readonly RemoveFromCartCommandHandler _handler;

        public RemoveFromCartCommandHandlerTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();

            _validator = Substitute.For<IValidator<RemoveFromCartCommand>>();

            _handler = new RemoveFromCartCommandHandler(_cartRepository, _validator);
        }

        [Fact]
        public async Task RemoveFromCartCommand_ValidCommand_ShouldCallRemoveFromCartOnce()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var command = new RemoveFromCartCommand(userId, productId);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _cartRepository.Received(1).RemoveFromCart(userId, productId);
        }

        [Fact]
        public async Task RemoveFromCartCommand_InvalidCommand_ShouldThrowException_AndNotCallRemoveFromCart()
        {
            // Arrange
            var userId = Guid.Empty; 
            var productId = Guid.Empty; 
            var command = new RemoveFromCartCommand(userId, productId);

            var failures = new[]
            {
                new ValidationFailure("UserId", "UserId must not be empty."),
                new ValidationFailure("ProductId", "ProductId must not be empty.")
            };
            var validationResult = new ValidationResult(failures);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(validationResult));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.Contains("UserId must not be empty.", exception.Message);
            Assert.Contains("ProductId must not be empty.", exception.Message);

            await _cartRepository.DidNotReceive().RemoveFromCart(Arg.Any<Guid>(), Arg.Any<Guid>());
        }
    }
}
