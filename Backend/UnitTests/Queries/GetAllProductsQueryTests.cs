using Application.DTOs.Product;
using Application.UseCases.Product.Queries.GetAllProducts;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Queries
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAllProductsQuery> _validator;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _repository = Substitute.For<IProductRepository>();

            _mapper = Substitute.For<IMapper>();

            _validator = Substitute.For<IValidator<GetAllProductsQuery>>();

            _handler = new GetAllProductsQueryHandler(_repository, _mapper, _validator);
        }

        public static IEnumerable<object[]> GetFilteredProductsData =>
            new List<object[]>
            {
                new object[] { "Electronics", (decimal?)100.00m, (decimal?)1500.00m, (int?)4 },
                new object[] { "Books", null, (decimal?)50.00m, null },
                new object[] { null, (decimal?)20.00m, null, (int?)2 },
            };

        [Fact]
        public async Task GetAllProductsQuery_ShouldReturnListOfProducts()
        {
            // Arrange
            var products = GenerateProducts();
            _repository.GetAllProducts().Returns(products);

            var query = new GetAllProductsQuery(null, null, null, null, 0, 10);
            var productDTOs = GenerateProductDTOs(products);

            _mapper.Map<IEnumerable<ProductDTO>>(Arg.Any<IEnumerable<Product>>()).Returns(productDTOs);

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(products.Count);
            result.TotalItems.Should().Be(products.Count);
            result.PageNumber.Should().Be(0);
            result.PageSize.Should().Be(10);

            for (int i = 0; i < products.Count; i++)
            {
                var dto = result.Data.ElementAt(i);
                var product = products[i];
                dto.Id.Should().Be(product.Id);
                dto.Name.Should().Be(product.Name);
                dto.Type.Should().Be(product.Type);
                dto.Description.Should().Be(product.Description);
                dto.Price.Should().Be(product.Price);
                dto.Review.Should().Be(product.Review);
            }

            _mapper.Received(1).Map<IEnumerable<ProductDTO>>(Arg.Any<IEnumerable<Product>>());
        }

        [Fact]
        public async Task GetAllProductsQuery_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var products = new List<Product>();
            _repository.GetAllProducts().Returns(products);

            var query = new GetAllProductsQuery(null, null, null, null, 0, 10);
            var productDTOs = new List<ProductDTO>();

            _mapper.Map<IEnumerable<ProductDTO>>(Arg.Any<IEnumerable<Product>>()).Returns(productDTOs);

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
            result.PageNumber.Should().Be(0);
            result.PageSize.Should().Be(10);

            _mapper.Received(1).Map<IEnumerable<ProductDTO>>(Arg.Any<IEnumerable<Product>>());
        }

        [Theory]
        [MemberData(nameof(GetFilteredProductsData))]
        public async Task GetAllProductsQuery_ShouldReturnFilteredProducts(string? type, decimal? minPrice, decimal? maxPrice, int? minReview)
        {
            // Arrange
            var allProducts = GenerateProducts();
            _repository.GetAllProducts().Returns(allProducts);

            var query = new GetAllProductsQuery(type, minPrice, maxPrice, minReview, 0, 10);

            var filteredProducts = allProducts.AsQueryable();

            if (!string.IsNullOrEmpty(type))
                filteredProducts = filteredProducts.Where(p => p.Type == type);

            if (minPrice.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Price <= maxPrice.Value);

            if (minReview.HasValue)
                filteredProducts = filteredProducts.Where(p => p.Review >= minReview.Value);

            var finalFilteredList = filteredProducts.ToList();

            // Apply pagination
            var pagedProducts = finalFilteredList
                .Skip(query.PageNumber * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var productDTOs = GenerateProductDTOs(pagedProducts);

            _mapper.Map<IEnumerable<ProductDTO>>(Arg.Any<IEnumerable<Product>>()).Returns(productDTOs);

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(pagedProducts.Count);
            result.TotalItems.Should().Be(finalFilteredList.Count);
            result.PageNumber.Should().Be(0);
            result.PageSize.Should().Be(10);

            for (int i = 0; i < pagedProducts.Count; i++)
            {
                var dto = result.Data.ElementAt(i);
                var product = pagedProducts[i];
                dto.Id.Should().Be(product.Id);
                dto.Name.Should().Be(product.Name);
                dto.Type.Should().Be(product.Type);
                dto.Description.Should().Be(product.Description);
                dto.Price.Should().Be(product.Price);
                dto.Review.Should().Be(product.Review);
            }

            _mapper.Received(1).Map<IEnumerable<ProductDTO>>(Arg.Any<IEnumerable<Product>>());
        }

        [Fact]
        public async Task GetAllProductsQuery_InvalidQuery_ShouldThrowException_AndNotCallRepository()
        {
            // Arrange
            var query = new GetAllProductsQuery(null, -10.00m, 5.00m, 6, 0, 10); // Invalid MinPrice and MinReview

            var failures = new[]
            {
                new ValidationFailure("MinPrice", "MinPrice must be greater than or equal to zero."),
                new ValidationFailure("MinReview", "MinReview must be between 0 and 5.")
            };
            var validationResult = new ValidationResult(failures);
            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(validationResult));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(query, CancellationToken.None));

            exception.Message.Should().Contain("Invalid GetAllProductsQuery");
            exception.Message.Should().Contain("MinPrice must be greater than or equal to zero.");
            exception.Message.Should().Contain("MinReview must be between 0 and 5.");

            await _repository.DidNotReceive().GetAllProducts();
        }

        private List<Product> GenerateProducts()
        {
            return new List<Product>
            {
                new(Guid.NewGuid(), "Electronics", "Laptop", "High-end laptop", 1500.00m, 5, new Guid()),
                new(Guid.NewGuid(), "Electronics", "Smartphone", "Latest smartphone", 800.00m, 4, new Guid()),
                new(Guid.NewGuid(), "Books", "C# Programming", "Learn C#", 45.00m, 3, new Guid()),
                new(Guid.NewGuid(), "Books", "ASP.NET Core", "Web development", 55.00m, 4, new Guid()),
                new(Guid.NewGuid(), "Electronics", "Headphones", "Noise-cancelling", 200.00m, 4, new Guid())
            };
        }

        private List<ProductDTO> GenerateProductDTOs(IEnumerable<Product> products)
        {
            return products.Select(p =>
                new ProductDTO(p.Id, p.Type, p.Name, p.Description, p.Price, p.Review)
            ).ToList();
        }
    }
}
