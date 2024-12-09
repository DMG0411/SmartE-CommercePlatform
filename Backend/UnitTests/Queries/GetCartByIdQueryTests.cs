using Application.DTOs.Cart;
using Application.DTOs.Product;
using Application.DTOs.User;
using Application.UseCases.Cart.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace UnitTests.Queries
{
    public class GetCartByIdQueryTests
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;

        public GetCartByIdQueryTests()
        {
            cartRepository = Substitute.For<ICartRepository>();
            mapper = Substitute.For<IMapper>();
        }

        //[Fact]
        //public async void GetCartByIdQuery_ShouldReturnCartDTO()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var cart = GenerateCart();
        //    cartRepository.GetCartById(userId).Returns(cart);

        //    var query = new GetCartByIdQuery(userId);
        //    GenerateCartDTO(cart);
        //    var handler = new GetCartByIdQueryHandler(cartRepository, mapper);

        //    // Act
        //    var result = await handler.Handle(query, CancellationToken.None);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Id.Should().Be(cart.Id);
        //    result.User.Id.Should().Be(cart.User.Id);
        //    result.User.Username.Should().Be(cart.User.Username);
        //    result.User.Email.Should().Be(cart.User.Email);
        //    result.Products.Should().HaveCount(cart.Products.Count);
        //    result.Products.First().Id.Should().Be(cart.Products.First().Id);
        //    result.Products.First().Name.Should().Be(cart.Products.First().Name);
        //    result.Products.First().Type.Should().Be(cart.Products.First().Type);
        //    result.Products.First().Description.Should().Be(cart.Products.First().Description);
        //    result.Products.First().Price.Should().Be(cart.Products.First().Price);
        //    result.Products.First().Review.Should().Be(cart.Products.First().Review);
        //}

        private void GenerateCartDTO(Cart cart)
        {
            mapper.Map<CartDTO>(Arg.Any<Cart>()).Returns(new CartDTO
            {
                Id = cart.Id,
                User = new UserDTO(cart.User.Id, cart.User.Username,cart.User.Password,cart.User.Email),
                Products = cart.Products.Select(p => new ProductDTO(
                    p.Id, p.Type, p.Name, p.Description, p.Price, p.Review)).ToList()
            });
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
                },
                Products = new List<Product>
                {
                    new Product(Guid.NewGuid(), "Electronics", "Smartphone", "Latest model", 999.99m, 5),
                    new Product(Guid.NewGuid(), "Accessories", "Phone Case", "Durable and stylish", 19.99m, 4)
                }
            };
        }
    }
}
