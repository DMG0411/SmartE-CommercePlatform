using Application.DTOs.User;
using Application.UseCases.User.Commands.CreateUser;
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
    public class CreateUserCommandHandlerTests
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserCommand> _validator;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _repository = Substitute.For<IUserRepository>();

            _mapper = Substitute.For<IMapper>();

            _validator = Substitute.For<IValidator<CreateUserCommand>>();

            _handler = new CreateUserCommandHandler(_repository, _mapper, _validator);
        }

        [Fact]
        public async Task CreateUserCommand_ValidCommand_ShouldReturnNewUserId()
        {
            // Arrange
            var userDTO = new CreateUserDTO("Username", "Password123", "email@example.com");
            var command = new CreateUserCommand(userDTO);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(new ValidationResult()));

            var userEntity = new User
            {
                Username = userDTO.Username,
                Password = userDTO.Password,
                Email = userDTO.Email
            };
            _mapper.Map<User>(userDTO).Returns(userEntity);

            var expectedUserId = Guid.NewGuid();
            _repository.CreateUser(userEntity).Returns(Task.FromResult(expectedUserId));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedUserId, result);
            await _repository.Received(1).CreateUser(userEntity);
        }

        [Fact]
        public async Task CreateUserCommand_InvalidCommand_ShouldReturnEmptyGuid_AndNotCallCreateUser()
        {
            // Arrange
            var userDTO = new CreateUserDTO("", "short", "invalid-email"); 
            var command = new CreateUserCommand(userDTO);

            var failures = new[]
            {
                new ValidationFailure("User.Username", "Username is required"),
                new ValidationFailure("User.Password", "Password must be at least 10 characters long"),
                new ValidationFailure("User.Email", "Email must be valid")
            };
            var validationResult = new ValidationResult(failures);

            _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
                      .Returns(Task.FromResult(validationResult));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Guid.Empty, result);
            await _repository.DidNotReceive().CreateUser(Arg.Any<User>());
        }
    }
}
