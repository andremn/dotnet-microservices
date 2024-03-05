using Users.Application.Services.Results;
using Users.Domain.Models;

namespace Users.Application.Services.Interfaces;

public interface IUserService
{
    Task<LoginUserResult> LoginAsync(string username, string password);

    Task<CreateUserResult> CreateUserAsync(User user, string password);

    Task<GetUserResult> GetUserByEmailAsync(string email);
}
