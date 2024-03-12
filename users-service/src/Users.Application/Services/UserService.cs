using FluentValidation;
using Users.Application.Services.Interfaces;
using Users.Application.Services.Results;
using Users.Domain.Models;
using Users.Domain.Repository;

namespace Users.Application.Services;

public class UserService(
    IUserSignInService signInManager,
    IUserRepository userRepository,
    IValidator<User> userValidator
) : IUserService
{
    public async Task<LoginUserResult> LoginAsync(string email, string password)
    {
        var user = await userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return new LoginUserResult(null);
        }

        var success = await signInManager.SignInAsync(email, password);

        return new LoginUserResult(success ? user : null);
    }

    public async Task<CreateUserResult> CreateAsync(User user, string password)
    {
        var validationResult = userValidator.Validate(user);

        if (!validationResult.IsValid)
        {
            return new CreateUserResult(false, null,
                validationResult.Errors.ToDictionary(
                    k => k.PropertyName, 
                    v => v.ErrorMessage));
        }

        var createdUser = await userRepository.CreateAsync(user, password);

        return createdUser is null ? 
            new CreateUserResult(false, null, []) : 
            new CreateUserResult(true, createdUser, []);
    }
}
