using Application.DTOs;
using Application.UseCases.Product.Queries.GetAllProducts;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace UnitTests.Queries
{
    public class GetAllProductsQueryTests
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public GetAllProductsQueryTests()
        {
            repository = Substitute.For<IProductRepository>();
            mapper = Substitute.For<IMapper>();
        }


        [Fact]
        public async void GetAllProductsQuery_ShouldReturnListOfProducts()
        {
            List<Product> products = GenerateProduct();
            repository.GetAllProducts().Returns(products);

            var query = new GetAllProductsQuery();
            GenerateProductDTO(products);
            var handler = new GetAllProductsQueryHandler(repository, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products.First().Id, result.First().Id);
            Assert.Equal(products.First().Name, result.First().Name);
            Assert.Equal(products.First().Type, result.First().Type);
            Assert.Equal(products.First().Description, result.First().Description);
            Assert.Equal(products.First().Price, result.First().Price);
            Assert.Equal(products.First().Review, result.First().Review);
            Assert.Equal(products.Last().Id, result.Last().Id);
            Assert.Equal(products.Last().Name, result.Last().Name);
            Assert.Equal(products.Last().Type, result.Last().Type);
            Assert.Equal(products.Last().Description, result.Last().Description);
            Assert.Equal(products.Last().Price, result.Last().Price);
            Assert.Equal(products.Last().Review, result.Last().Review);

            // Assert
            result.Should().NotBeNull();
            result.First().Id.Should().Be(products.First().Id);
            result.First().Name.Should().Be(products.First().Name);
            result.First().Type.Should().Be(products.First().Type);
            result.First().Description.Should().Be(products.First().Description);
            result.First().Price.Should().Be(products.First().Price);
            result.First().Review.Should().Be(products.First().Review);
            result.Should().NotBeNull();
            result.Last().Id.Should().Be(products.Last().Id);
            result.Last().Name.Should().Be(products.Last().Name);
            result.Last().Type.Should().Be(products.Last().Type);
            result.Last().Description.Should().Be(products.Last().Description);
            result.Last().Price.Should().Be(products.Last().Price);
            result.Last().Review.Should().Be(products.Last().Review);
        }

        private void GenerateProductDTO(List<Product> products)
        {
            mapper.Map<List<ProductDTO>>(Arg.Any<List<Product>>()).Returns(new List<ProductDTO>
            {
                new(products.First().Id, products.First().Type, products.First().Name, products.First().Description, products.First().Price, products.First().Review),
                new(products.Last().Id, products.Last().Type, products.Last().Name, products.Last().Description, products.Last().Price, products.Last().Review)
            });
        }

        private List<Product> GenerateProduct()
        {
            return new List<Product>
            {
                new(Guid.NewGuid(), "Type", "Product 1", "Description", 10.22, 3),
                new(Guid.NewGuid(), "Type", "Product 2", "Description", 10.22, 3)
            };
        }
    }
}
