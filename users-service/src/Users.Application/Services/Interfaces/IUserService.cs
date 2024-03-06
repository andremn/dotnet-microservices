using Users.Application.Services.Results;
using Users.Domain.Models;

namespace Users.Application.Services.Interfaces;

public interface IUserService
{
    Task<LoginUserResult> LoginAsync(string email, string password);

    Task<CreateUserResult> CreateAsync(User user, string password);
}
