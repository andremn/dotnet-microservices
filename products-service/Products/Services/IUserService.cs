using Products.Model;
using Products.Services.Results;

namespace Products.Services;

public interface IUserService
{
    Task<LoginUserResult> LoginAsync(string username, string password);

    Task<CreateUserResult> CreateUserAsync(User user, string password);

    Task<GetUserResult> GetUserByEmailAsync(string email);
}
