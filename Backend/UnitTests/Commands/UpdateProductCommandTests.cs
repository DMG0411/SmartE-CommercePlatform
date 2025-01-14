using Application.DTOs;
using Application.DTOs.Product;
using Application.UseCases.Product.Commands.UpdateProduct;
using AutoMapper;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace UnitTests.Commands
{
    public class UpdateProductCommandTests
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public UpdateProductCommandTests()
        {
            repository = Substitute.For<IProductRepository>();
            mapper = Substitute.For<IMapper>();
        }


        [Fact]
        public async void UpdateProductCommand_ValidCommand_ShouldPass()
        {
            var product = new ProductDTO
            (new Guid(),"Type", "Product 1", "Description", 10.22m, 3, new Guid());


            // Arrange
            var command = new UpdateProductCommand(product);
            var handler = new UpdateProductCommandHandler(repository, mapper);

            // Act
            await handler.Handle(command, CancellationToken.None);
        }

        [Fact]
        public async void UpdateProductCommand_InvalidCommand_ShouldFail()
        {
            var product = new ProductDTO
           (new Guid(), "Type", "Product 1", "Description", -1, 3, new Guid());

            // Arrange
            var command = new UpdateProductCommand(product);
            var handler = new UpdateProductCommandHandler(repository, mapper);

            // Act
            await handler.Handle(command, CancellationToken.None);
        }
    }
}
