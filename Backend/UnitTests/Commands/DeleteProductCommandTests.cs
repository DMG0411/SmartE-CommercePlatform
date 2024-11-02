using Application.UseCases.Product.Commands.DeleteProduct;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace UnitTests.Commands
{
    public class DeleteProductCommandTests
    {
        private readonly IProductRepository repository;

        public DeleteProductCommandTests()
        {
            repository = Substitute.For<IProductRepository>();
        }


        [Fact]
        public async void DeleteProductCommand_ValidCommand_ShouldPass()
        {
            // Arrange
            var command = new DeleteProductCommand(new Guid());
            var handler = new DeleteProductCommandHandler(repository);

            // Act
            await handler.Handle(command, CancellationToken.None);
        }

        [Fact]
        public async void DeleteProductCommand_InvalidCommand_ShouldFail()
        {
            // Arrange
            var command = new DeleteProductCommand(new Guid());
            var handler = new DeleteProductCommandHandler(repository);

            // Act
            await handler.Handle(command, CancellationToken.None);
        }
    }
}
