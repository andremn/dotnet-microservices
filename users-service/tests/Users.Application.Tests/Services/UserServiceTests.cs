using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Users.Application.Services;
using Users.Application.Services.Results;
using Users.Domain.Models;
using Users.Domain.Repository;

namespace Users.Application.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserSignInService> _userSignInServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IValidator<User>> _userValidatorMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userSignInServiceMock = new Mock<IUserSignInService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userValidatorMock = new Mock<IValidator<User>>();
        _userService = new UserService(_userSignInServiceMock.Object,
            _userRepositoryMock.Object, _userValidatorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidUser_ReturnsSuccessWithUserData()
    {
        // Arrange
        var password = "password";
        var userToCreate = new User(string.Empty, "User", "Test", "user@mail.com");
        var expectedUser = userToCreate with { Id = "user-1" };
        var expectedResult = new CreateUserResult(true, expectedUser, []);

        _userRepositoryMock.Setup(x => x.CreateAsync(userToCreate, password))
            .ReturnsAsync(expectedUser);

        _userValidatorMock.Setup(x => x.Validate(userToCreate))
            .Returns(new ValidationResult());

        // Act
        var actualResult = await _userService.CreateAsync(userToCreate, password);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateAsync_InvalidUser_ReturnsError()
    {
        // Arrange
        var password = "password";
        var userToCreate = new User(string.Empty, "User", "Test", "user@mail.com");
        var expectedUser = userToCreate with { Id = "user-1" };

        var errors = new Dictionary<string, string>
        {
            ["Email"] = "'Email' is not a valid email address",
            ["Password"] = "'Password' cannot be empty"
        };

        var expectedResult = new CreateUserResult(false, null, errors);

        _userRepositoryMock.Setup(x => x.CreateAsync(userToCreate, password))
            .ReturnsAsync(expectedUser);

        _userValidatorMock.Setup(x => x.Validate(userToCreate))
            .Returns(new ValidationResult(errors.Select(x => new ValidationFailure(x.Key, x.Value))));

        // Act
        var actualResult = await _userService.CreateAsync(userToCreate, password);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateAsync_ExistingUserEmail_ReturnsError()
    {
        // Arrange
        var password = "password";
        var userToCreate = new User(string.Empty, "User", "Test", "user@mail.com");
        var expectedResult = new CreateUserResult(false, null, []);

        _userRepositoryMock.Setup(x => x.CreateAsync(userToCreate, password))
            .ReturnsAsync((User?)null);

        _userValidatorMock.Setup(x => x.Validate(userToCreate))
            .Returns(new ValidationResult());

        // Act
        var actualResult = await _userService.CreateAsync(userToCreate, password);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task LoginAsync_ValidLogin_ReturnsSuccessWithUserData()
    {
        // Arrange
        var email = "user@mail.com";
        var password = "password";
        var expectedUser = new User("user-1", "User", "Test", "user@mail.com");
        var expectedResult = new LoginUserResult(expectedUser);

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(expectedUser);

        _userSignInServiceMock.Setup(x => x.SignInAsync(email, password))
            .ReturnsAsync(true);

        // Act
        var actualResult = await _userService.LoginAsync(email, password);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task LoginAsync_InvalidLogin_ReturnsSuccessWithUserData()
    {
        // Arrange
        var email = "user@mail.com";
        var password = "password";
        var expectedUser = new User("user-1", "User", "Test", "user@mail.com");
        var expectedResult = new LoginUserResult(null);

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(expectedUser);

        _userSignInServiceMock.Setup(x => x.SignInAsync(email, password))
            .ReturnsAsync(false);

        // Act
        var actualResult = await _userService.LoginAsync(email, password);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsNullUser()
    {
        // Arrange
        var email = "user@mail.com";
        var password = "password";
        var expectedResult = new LoginUserResult(null);

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync((User?)null);

        // Act
        var actualResult = await _userService.LoginAsync(email, password);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
