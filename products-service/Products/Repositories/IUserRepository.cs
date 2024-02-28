using Products.Model;

namespace Products.Repositories;

public interface IUserRepository
{
    Task<bool> LoginAsync(string email, string password);

    Task<bool> CreateAsync(User user, string password);

    Task<User?> FindByEmailAsync(string email);
}
