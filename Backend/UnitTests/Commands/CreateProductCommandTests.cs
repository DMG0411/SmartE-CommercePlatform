using Application.DTOs.Product;
using Application.UseCases.Product.Commands.CreateProduct;
using AutoMapper;
using Domain.Repositories;
using Domain.Entities; 
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Commands
{
    public class CreateProductCommandHandlerTests
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductCommand> _validator;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _repository = Substitute.For<IProductRepository>();

            _mapper = Substitute.For<IMapper>();

            _validator = Substitute.For<IValidator<CreateProductCommand>>();

            _handler = new CreateProductCommandHandler(_repository, _mapper, _validator);
        }

        [Fact]
        public async Task CreateProductCommand_ValidCommand_ShouldReturnNewProductId()
        {
            // Arrange
            var productDTO = new CreateProductDTO("Electronics", "Smartphone", "Latest model", 999.99m, 10); // Review = 10
            var command = new CreateProductCommand(productDTO);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            var productEntity = new Product
            {
                Type = productDTO.Type,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                Review = productDTO.Review
            };
            _mapper.Map<Product>(productDTO).Returns(productEntity);

            var expectedProductId = Guid.NewGuid();
            _repository.CreateProduct(productEntity).Returns(Task.FromResult(expectedProductId));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedProductId, result);
            await _repository.Received(1).CreateProduct(productEntity);
        }

        [Fact]
        public async Task CreateProductCommand_InvalidCommand_ShouldReturnEmptyGuid_AndNotCallAddProductAsync()
        {
            // Arrange
            var productDTO = new CreateProductDTO("Electronics", "Smartphone", "Latest model", -10m, -1); // Invalid price and review
            var command = new CreateProductCommand(productDTO);

            var failures = new[]
            {
                new ValidationFailure("Product.Price", "Price must be greater than zero."),
                new ValidationFailure("Product.Review", "Review cannot be negative.")
            };
            var validationResult = new ValidationResult(failures);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(validationResult));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Guid.Empty, result);
            await _repository.DidNotReceive().CreateProduct(Arg.Any<Product>());
        }
    }
}
