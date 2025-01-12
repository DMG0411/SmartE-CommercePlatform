using Application.DTOs.Cart;
using Application.DTOs.Product;
using Application.DTOs.User;
using Application.UseCases.Cart.Queries.GetCartById;
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
using Application.UseCases.Cart.Queries;

namespace UnitTests.Queries
{
    public class GetCartByIdQueryTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetCartByIdQuery> _validator;
        private readonly GetCartByIdQueryHandler _handler;

        public GetCartByIdQueryTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Cart, CartDTO>();

                cfg.CreateMap<User, UserDTO>()
                    .ConstructUsing(src => new UserDTO())
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

                cfg.CreateMap<Product, ProductDTO>();
            });

            configuration.AssertConfigurationIsValid();

            _mapper = new Mapper(configuration);

            _validator = Substitute.For<IValidator<GetCartByIdQuery>>();

            _handler = new GetCartByIdQueryHandler(_cartRepository, _mapper, _validator);
        }

        [Fact]
        public async Task GetCartByIdQuery_ShouldReturnCartDTO_WhenQueryIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cart = GenerateCart();
            _cartRepository.GetCartById(userId).Returns(cart);

            var query = new GetCartByIdQuery(userId);

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(cart.Id);
            result.User.Should().NotBeNull();
            result.User.Id.Should().Be(cart.User.Id);
            result.User.Username.Should().Be(cart.User.Username);
            result.User.Email.Should().Be(cart.User.Email);
            result.Products.Should().HaveCount(cart.Products.Count);

            for (int i = 0; i < cart.Products.Count; i++)
            {
                var expectedProduct = cart.Products.ElementAt(i);
                var actualProduct = result.Products.ElementAt(i);

                actualProduct.Id.Should().Be(expectedProduct.Id);
                actualProduct.Name.Should().Be(expectedProduct.Name);
                actualProduct.Type.Should().Be(expectedProduct.Type);
                actualProduct.Description.Should().Be(expectedProduct.Description);
                actualProduct.Price.Should().Be(expectedProduct.Price);
                actualProduct.Review.Should().Be(expectedProduct.Review);
            }

            await _cartRepository.Received(1).GetCartById(userId);
        }

        [Fact]
        public async Task GetCartByIdQuery_ShouldThrowException_WhenQueryIsInvalid()
        {
            // Arrange
            var invalidUserId = Guid.Empty; // Invalid GUID
            var query = new GetCartByIdQuery(invalidUserId);

            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("UserId", "UserId must be provided."),
                new ValidationFailure("UserId", "UserId cannot be empty.")
            };
            var validationResult = new ValidationResult(failures);
            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(validationResult));

            // Act
            Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid GetCartByIdQuery: UserId must be provided.; UserId cannot be empty.");

            await _cartRepository.DidNotReceive().GetCartById(Arg.Any<Guid>());
        }

        [Fact]
        public async Task GetCartByIdQuery_ShouldReturnNull_WhenCartDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _cartRepository.GetCartById(userId).Returns((Cart)null);

            var query = new GetCartByIdQuery(userId);

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            await _cartRepository.Received(1).GetCartById(userId);
        }

        [Fact]
        public async Task GetCartByIdQuery_ShouldReturnCartDTO_WithNoProducts_WhenCartHasNoProducts()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Jane Doe",
                    Email = "jane.doe@example.com",
                },
                Products = new List<Product>() // No products
            };
            _cartRepository.GetCartById(userId).Returns(cart);

            var query = new GetCartByIdQuery(userId);

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(cart.Id);
            result.User.Should().NotBeNull();
            result.User.Id.Should().Be(cart.User.Id);
            result.User.Username.Should().Be(cart.User.Username);
            result.User.Email.Should().Be(cart.User.Email);
            result.Products.Should().BeEmpty();

            await _cartRepository.Received(1).GetCartById(userId);
        }

        [Fact]
        public async Task GetCartByIdQuery_ShouldThrowException_WhenRepositoryThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _cartRepository.GetCartById(userId).Returns<Task<Cart>>(x => throw new Exception("Database error"));

            var query = new GetCartByIdQuery(userId);

            _validator.ValidateAsync(query, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            // Act
            Func<Task> act = async () => { await _handler.Handle(query, CancellationToken.None); };

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Database error");

            await _cartRepository.Received(1).GetCartById(userId);
        }

        private Cart GenerateCart()
        {
            return new Cart
            {
                Id = Guid.NewGuid(),
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "John Doe",
                    Email = "john.doe@example.com",
                    // Assuming Password is set elsewhere or nullable
                },
                Products = new List<Product>
                {
                    new Product(Guid.NewGuid(), "Electronics", "Smartphone", "Latest model", 999.99m, 5, new Guid()),
                    new Product(Guid.NewGuid(), "Accessories", "Phone Case", "Durable and stylish", 19.99m, 4, new Guid())
                }
            };
        }
    }
}
