using Application.DTOs.Product;
using Application.UseCases.Product.Commands.CreateProduct;
using AutoMapper;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace UnitTests.Commands
{
    public class CreateProductCommandTests
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public CreateProductCommandTests()
        {
            repository = Substitute.For<IProductRepository>();
            mapper = Substitute.For<IMapper>();
        }


        [Fact]
        public async void CreateProductCommand_ValidCommand_ShouldPass()
        {
            var product = new CreateProductDTO
            ("Type", "Product 1", "Description", 10.22m, 3);


            // Arrange
            var command = new CreateProductCommand(product);
            var handler = new CreateProductCommandHandler(repository, mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        //[Fact]
        //public async void CreateProductCommand_InvalidCommand_ShouldFail()
        //{
        //    var product = new CreateProductDTO
        //   ("Type", "Product 1", "Description", -1, 3);

        //    // Arrange
        //    var command = new CreateProductCommand(product);
        //    var handler = new CreateProductCommandHandler(repository, mapper);

        //    // Act
        //    var result = await handler.Handle(command, CancellationToken.None);

        //    // Assert
        //    Assert.Null(result);
        //}
    }
}
