using Application.DTOs.Product;
using Application.UseCases.Product.Queries.GetProductById;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace UnitTests.Queries
{
    public class GetProductByIdQueryTests
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public GetProductByIdQueryTests()
        {
            repository = Substitute.For<IProductRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async void GetProductByIdQuery_ShouldReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = GenerateProduct(productId);
            repository.GetProductById(productId).Returns(product);

            var query = new GetProductByIdQuery(productId);
            GenerateProductDTO(product);

            var handler = new GetProductByIdQueryHandler(repository, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Type, result.Type);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Review, result.Review);

            result.Should().NotBeNull();
            result.Id.Should().Be(product.Id);
            result.Name.Should().Be(product.Name);
            result.Type.Should().Be(product.Type);
            result.Description.Should().Be(product.Description);
            result.Price.Should().Be(product.Price);
            result.Review.Should().Be(product.Review);
        }

        private void GenerateProductDTO(Product product)
        {
            mapper.Map<ProductDTO>(Arg.Any<Product>()).Returns(new ProductDTO(
                product.Id,
                product.Type,
                product.Name,
                product.Description,
                product.Price,
                product.Review,
                product.UserId));
        }

        private Product GenerateProduct(Guid id)
        {
            return new Product(
                id,
                "Type",
                "Test Product",
                "Description of product",
                99.99m,
                4,
                new Guid("2fb59ab9-6050-47d2-96a3-de0f09b02e5f"));
        }
    }
}
