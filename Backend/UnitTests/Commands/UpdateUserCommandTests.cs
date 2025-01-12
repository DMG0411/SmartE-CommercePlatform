using Application.DTOs.User;
using Application.UseCases.User.Commands.UpdateUser;
using AutoMapper;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace UnitTests.Commands
{
    public class UpdateUserCommandTests
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UpdateUserCommandTests()
        {
            repository = Substitute.For<IUserRepository>();
            mapper = Substitute.For<IMapper>();
        }


        [Fact]
        public async void UpdateUserCommand_ValidCommand_ShouldPass()
        {
            var product = new UserDTO
            (new Guid(), "Username", "Password", "Email", "PhoneNumber", "City");


            // Arrange
            var command = new UpdateUserCommand(product);
            var handler = new UpdateUserCommandHandler(repository, mapper);

            // Act
            await handler.Handle(command, CancellationToken.None);
        }

        [Fact]
        public async void UpdateUserCommand_InvalidCommand_ShouldFail()
        {
            var product = new UserDTO
           (new Guid(), "Username", "Password", "Email", "PhoneNumber", "City");

            // Arrange
            var command = new UpdateUserCommand(product);
            var handler = new UpdateUserCommandHandler(repository, mapper);

            // Act
            await handler.Handle(command, CancellationToken.None);
        }
    }
}
