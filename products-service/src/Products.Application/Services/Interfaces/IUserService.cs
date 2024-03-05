using Products.Application.Services.Results;
using Products.Domain.Models;

namespace Products.Application.Services.Interfaces;

public interface IUserService
{
    Task<LoginUserResult> LoginAsync(string username, string password);

    Task<CreateUserResult> CreateUserAsync(User user, string password);

    Task<GetUserResult> GetUserByEmailAsync(string email);
}
