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
        var result = await _userRepository.LoginAsync(username, password);

        return new LoginUserResult(result);
    }

    public async Task<CreateUserResult> CreateUserAsync(User user, string password)
    {
        var result = await _userRepository.CreateAsync(user, password);

        return new CreateUserResult(result);
    }

    public async Task<GetUserResult> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.FindByEmailAsync(email);

        return new GetUserResult(user);
    }
}
