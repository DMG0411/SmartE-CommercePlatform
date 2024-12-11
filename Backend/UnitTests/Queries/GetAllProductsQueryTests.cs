using Application.DTOs.Product;
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

        //[Fact]
        //public async Task GetAllProductsQuery_ShouldReturnListOfProducts()
        //{
        //    // Arrange
        //    List<Product> products = GenerateProduct();
        //    repository.GetFilteredProducts(null, null, null, null).Returns(products);

        //    var query = new GetAllProductsQuery(null, null, null, null);
        //    GenerateProductDTO(products);
        //    var handler = new GetAllProductsQueryHandler(repository, mapper);

        //    // Act
        //    var result = await handler.Handle(query, CancellationToken.None);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Data.Should().HaveCount(products.Count);
        //    result.Data.First().Id.Should().Be(products.First().Id);
        //    result.Data.First().Name.Should().Be(products.First().Name);
        //    result.Data.First().Type.Should().Be(products.First().Type);
        //    result.Data.First().Description.Should().Be(products.First().Description);
        //    result.Data.First().Price.Should().Be(products.First().Price);
        //    result.Data.First().Review.Should().Be(products.First().Review);
        //    result.Data.Last().Id.Should().Be(products.Last().Id);
        //    result.Data.Last().Name.Should().Be(products.Last().Name);
        //    result.Data.Last().Type.Should().Be(products.Last().Type);
        //    result.Data.Last().Description.Should().Be(products.Last().Description);
        //    result.Data.Last().Price.Should().Be(products.Last().Price);
        //    result.Data.Last().Review.Should().Be(products.Last().Review);

        //    mapper.Received(1).Map<IEnumerable<ProductDTO>>(products);
        //}

        //[Fact]
        //public async Task GetAllProductsQuery_ShouldReturnEmptyList_WhenNoProductsExist()
        //{
        //    // Arrange
        //    List<Product> products = new List<Product>();
        //    repository.GetFilteredProducts(null, null, null, null).Returns(products);

        //    var query = new GetAllProductsQuery(null, null, null, null);
        //    GenerateProductDTO(products);
        //    var handler = new GetAllProductsQueryHandler(repository, mapper);

        //    // Act
        //    var result = await handler.Handle(query, CancellationToken.None);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Data.Should().BeEmpty();

        //    mapper.Received(1).Map<IEnumerable<ProductDTO>>(products);
        //}

        //[Theory]
        //[InlineData("Electronics", 100.00, 500.00, 4)]
        //[InlineData("Books", null, 50.00, null)]
        //[InlineData(null, 20.00, null, 2)]
        //public async Task GetAllProductsQuery_ShouldReturnFilteredProducts(string? type, decimal? minPrice, decimal? maxPrice, int? minReview)
        //{
        //    // Arrange
        //    var products = new List<Product>
        //    {
        //        new(Guid.NewGuid(), "Electronics", "Laptop", "High-end laptop", 1500.00m, 5),
        //        new(Guid.NewGuid(), "Electronics", "Smartphone", "Latest smartphone", 800.00m, 4),
        //        new(Guid.NewGuid(), "Books", "C# Programming", "Learn C#", 45.00m, 3)
        //    };

        //    repository.GetFilteredProducts(type, minPrice, maxPrice, minReview).Returns(
        //        products.Where(p =>
        //            (type == null || p.Type == type) &&
        //            (!minPrice.HasValue || p.Price >= minPrice.Value) &&
        //            (!maxPrice.HasValue || p.Price <= maxPrice.Value) &&
        //            (!minReview.HasValue || p.Review >= minReview.Value)
        //        ).ToList()
        //    );

        //    var query = new GetAllProductsQuery(type, minPrice, maxPrice, minReview);
        //    var filteredProducts = products.Where(p =>
        //        (type == null || p.Type == type) &&
        //        (!minPrice.HasValue || p.Price >= minPrice.Value) &&
        //        (!maxPrice.HasValue || p.Price <= maxPrice.Value) &&
        //        (!minReview.HasValue || p.Review >= minReview.Value)
        //    ).ToList();
        //    GenerateProductDTO(filteredProducts);
        //    var handler = new GetAllProductsQueryHandler(repository, mapper);

        //    // Act
        //    var result = await handler.Handle(query, CancellationToken.None);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Data.Should().HaveCount(products.Count); ;
        //    foreach (var dto in result.Data)
        //    {
        //        var product = filteredProducts.FirstOrDefault(p => p.Id == dto.Id);
        //        product.Should().NotBeNull();
        //        dto.Name.Should().Be(product.Name);
        //        dto.Type.Should().Be(product.Type);
        //        dto.Price.Should().Be(product.Price);
        //        dto.Review.Should().Be(product.Review);
        //    }

        //    mapper.Received(1).Map<IEnumerable<ProductDTO>>(Arg.Is<IEnumerable<Product>>(p => p.SequenceEqual(filteredProducts)));

        //}

        private void GenerateProductDTO(List<Product> products)
        {
            mapper.Map<IEnumerable<ProductDTO>>(Arg.Any<IEnumerable<Product>>()).Returns(products.Select(p =>
                new ProductDTO(p.Id, p.Type, p.Name, p.Description, p.Price, p.Review)).ToList()
            );
        }

        private List<Product> GenerateProduct()
        {
            return new List<Product>
            {
                new(Guid.NewGuid(), "Type", "Product 1", "Description", 10.22m, 3),
                new(Guid.NewGuid(), "Type", "Product 2", "Description", 10.22m, 3)
            };
        }
    }
}
