using Application.UseCases.Cart.Commands.AddToCart;
using FluentValidation.TestHelper;
using Xunit;

public class AddToCartCommandValidatorTests
{
    private readonly AddToCartCommandValidator _validator;

    public AddToCartCommandValidatorTests()
    {
        _validator = new AddToCartCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
        // Arrange
        var command = new AddToCartCommand(Guid.Empty, Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.UserId)
              .WithErrorMessage("User ID must not be empty.");
    }

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        // Arrange
        var command = new AddToCartCommand(Guid.NewGuid(), Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ProductId)
              .WithErrorMessage("Product ID must not be empty.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new AddToCartCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
