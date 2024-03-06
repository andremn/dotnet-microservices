using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Users.Api.Configurations;
using Users.Api.Controllers.Users;
using Users.Application.Services.Interfaces;
using Users.Application.Services.Results;
using Users.Domain.Models;

namespace Products.Api.Tests.Controllers;

public class UsersControllerTests
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IOptionsSnapshot<JwtConfiguration>> _optionsSnapshotMock;
    private readonly UsersController _usersController;

    public UsersControllerTests()
    {
        _jwtConfiguration = new JwtConfiguration();
        _userServiceMock = new Mock<IUserService>();
        _optionsSnapshotMock = new Mock<IOptionsSnapshot<JwtConfiguration>>();

        _optionsSnapshotMock.SetupGet(x => x.Value).Returns(_jwtConfiguration);

        _usersController = new UsersController(
            _userServiceMock.Object,
            _optionsSnapshotMock.Object);
    }

    [Fact]
    public async Task Post_CreateWithSuccess_Returns200OkWithUserData()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest(FirstName: "User", LastName: "Test", Email: "user@mail.com", Password: "pass123");
        var expectedUser = new User(
            Id: string.Empty,
            FirstName: "User",
            LastName: "Test",
            Email: "user@mail.com");

        _userServiceMock.Setup(x => x.CreateAsync(expectedUser, createUserRequest.Password))
            .ReturnsAsync(new CreateUserResult(true));

        // Act
        var result = await _usersController.Post(createUserRequest);

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<User>().Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task Post_CreateWithError_Returns400BadRequest()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest(FirstName: "User", LastName: "Test", Email: "", Password: "");

        _userServiceMock.Setup(x => x.CreateAsync(It.Is<User>(u => u.Email == createUserRequest.Email), createUserRequest.Password))
            .ReturnsAsync(new CreateUserResult(false));

        // Act
        var result = await _usersController.Post(createUserRequest);

        // Assert
        var badRequestResult = result.Result.As<BadRequestResult>();

        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Post_LoginWithValidCredentials_Returns200OkWithTokenData()
    {
        // Arrange
        var loginRequest = new LoginRequest(Email: "user@mail.com", Password: "pass123");
        var expectedUser = new User(
            Id: string.Empty,
            FirstName: "User",
            LastName: "Test",
            Email: "user@mail.com");

        _jwtConfiguration.Key = "very-long-jwt-token-key-for-testing";

        _userServiceMock.Setup(x => x.LoginAsync(loginRequest.Email, loginRequest.Password))
            .ReturnsAsync(new LoginUserResult(expectedUser));

        // Act
        var result = await _usersController.Post(loginRequest);

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task Post_LoginFailed_Returns401Unauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest(Email: "user@mail.com", Password: "pass123");
        var expectedUser = new User(
            Id: "user-id-1",
            FirstName: "User",
            LastName: "Test",
            Email: "user@mail.com");

        _userServiceMock.Setup(x => x.LoginAsync(loginRequest.Email, loginRequest.Password))
            .ReturnsAsync(new LoginUserResult((User?)null));

        // Act
        var result = await _usersController.Post(loginRequest);

        // Assert
        var unauthorizedResult = result.Result.As<UnauthorizedResult>();

        unauthorizedResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }
}