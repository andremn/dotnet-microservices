using Users.Application.Services.Interfaces;
using Users.Application.Services.Results;
using Users.Domain.Models;
using Users.Domain.Repository;

namespace Users.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<LoginUserResult> LoginAsync(string username, string password)
    {
        var loggedUser = await _userRepository.LoginAsync(username, password);

        return new LoginUserResult(loggedUser);
    }

    public async Task<CreateUserResult> CreateAsync(User user, string password)
    {
        var result = await _userRepository.CreateAsync(user, password);

        return new CreateUserResult(result);
    }
}
