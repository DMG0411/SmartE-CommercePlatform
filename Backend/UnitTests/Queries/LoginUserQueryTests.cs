using Application.DTOs.User;
using Application.UseCases.User.Queries.LoginUser;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace UnitTests.Queries
{
    public class LoginUserQueryTests
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public LoginUserQueryTests()
        {
            userRepository = Substitute.For<IUserRepository>();
            mapper = Substitute.For<IMapper>();
        }

        //[Fact]
        //public async void LoginUserQuery_ValidCredentials_ShouldReturnUser()
        //{
        //    // Arrange
        //    var user = GenerateUser();
        //    var loginDTO = new LoginUserDTO(user.Email, "password123");
        //    userRepository.LoginUser(Arg.Any<User>()).Returns(user);

        //    var query = new LoginUserQuery(loginDTO);
        //    GenerateUserDTO(user);
        //    var handler = new LoginUserQueryHandler(userRepository, mapper);

        //    // Act
        //    var result = await handler.Handle(query, CancellationToken.None);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(user.Id, result.Id);
        //    Assert.Equal(user.Username, result.Username);
        //    Assert.Equal(user.Email, result.Email);

        //    result.Should().NotBeNull();
        //    result.Id.Should().Be(user.Id);
        //    result.Username.Should().Be(user.Username);
        //    result.Email.Should().Be(user.Email);
        //}

        [Fact]
        public async void LoginUserQuery_InvalidCredentials_ShouldReturnNull()
        {
            // Arrange
            var loginDTO = new LoginUserDTO("johndoe@example.com", "wrongpassword");
            userRepository.LoginUser(Arg.Any<User>()).Returns((User)null);

            var query = new LoginUserQuery(loginDTO);
            var handler = new LoginUserQueryHandler(userRepository, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        private void GenerateUserDTO(User user)
        {
            mapper.Map<UserDTO>(Arg.Any<User>()).Returns(new UserDTO(user.Id, user.Username, user.Email, user.Password));
        }

        private User GenerateUser()
        {
            return new User(Guid.NewGuid(), "John Doe", "password123", "johndoe@example.com");
        }
    }
}
