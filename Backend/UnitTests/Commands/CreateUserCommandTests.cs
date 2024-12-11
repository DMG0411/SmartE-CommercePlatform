using Application.DTOs.User;
using Application.UseCases.User.Commands.CreateUser;
using AutoMapper;
using Domain.Repositories;
using NSubstitute;
using Xunit;

namespace UnitTests.Commands
{
    public class CreateUserCommandTests
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public CreateUserCommandTests()
        {
            repository = Substitute.For<IUserRepository>();
            mapper = Substitute.For<IMapper>();
        }


        [Fact]
        public async void CreateUserCommand_ValidCommand_ShouldPass()
        {
            var user = new CreateUserDTO
            ("Username", "Password", "Email");


            // Arrange
            var command = new CreateUserCommand(user);
            var handler = new CreateUserCommandHandler(repository, mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        //[Fact]
        //public async void CreateUserCommand_InvalidCommand_ShouldFail()
        //{
        //    var user = new CreateUserDTO
        //   ("Username", "Password", "Email");

        //    // Arrange
        //    var command = new CreateUserCommand(user);
        //    var handler = new CreateUserCommandHandler(repository, mapper);

        //    // Act
        //    var result = await handler.Handle(command, CancellationToken.None);

        //    // Assert
        //    Assert.Null(result);
        //}
    }
}
