using Application.UseCases.Cart.Commands.AddToCart;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Commands
{
    public class AddToCartCommandTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IValidator<AddToCartCommand> _validator;
        private readonly AddToCartCommandHandler _handler;

        public AddToCartCommandTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();

            _validator = Substitute.For<IValidator<AddToCartCommand>>();

            _handler = new AddToCartCommandHandler(_cartRepository, _validator);
        }

        [Fact]
        public async Task AddToCartCommand_ValidCommand_ShouldCallAddToCartOnce()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var command = new AddToCartCommand(userId, productId);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _cartRepository.Received(1).AddToCart(userId, productId);
        }

        [Fact]
        public async Task AddToCartCommand_InvalidCommand_ShouldNotCallAddToCart()
        {
            // Arrange
            var userId = Guid.Empty; 
            var productId = Guid.Empty; 
            var command = new AddToCartCommand(userId, productId);

            var failures = new[]
            {
                new ValidationFailure("UserId", "User ID must not be empty."),
                new ValidationFailure("ProductId", "Product ID must not be empty.")
            };
            var validationResult = new ValidationResult(failures);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(validationResult));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _cartRepository.DidNotReceive().AddToCart(userId, productId);
        }
    }
}
