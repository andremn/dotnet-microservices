using Users.Domain.Models;

namespace Users.Domain.Repository;

public interface IUserRepository
{
    Task<User?> LoginAsync(string email, string password);

    Task<User?> CreateAsync(User user, string password);
}
