using FluentValidation;
using Users.Application.Services.Interfaces;
using Users.Application.Services.Results;
using Users.Domain.Models;
using Users.Domain.Repository;

namespace Users.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IValidator<User> userValidator
) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<LoginUserResult> LoginAsync(string email, string password)
    {
        var loggedUser = await _userRepository.LoginAsync(email, password);

        return new LoginUserResult(loggedUser);
    }

    public async Task<CreateUserResult> CreateAsync(User user, string password)
    {
        var validationResult = userValidator.Validate(user);

        if (validationResult.IsValid)
        {
            var createdUser = await _userRepository.CreateAsync(user, password);

            if (createdUser is null)
            {
                return new CreateUserResult(false, null, []);
            }

            return new CreateUserResult(true, createdUser, []);
        }

        return new CreateUserResult(false, null, validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage));
    }
}
