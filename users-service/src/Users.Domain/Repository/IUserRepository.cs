using Users.Domain.Models;

namespace Users.Domain.Repository;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> CreateAsync(User user, string password);
}
