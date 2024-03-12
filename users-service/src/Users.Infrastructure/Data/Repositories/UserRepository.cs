using Microsoft.AspNetCore.Identity;
using Users.Domain.Models;
using Users.Domain.Repository;
using Users.Infrastructure.Identity;

namespace Users.Infrastructure.Data.Repositories;

internal class UserRepository(
    UserManager<ApplicationUser> userManager
) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        var entity = await userManager.FindByEmailAsync(email);

        return entity is null ? null : new User(entity.Id, entity.FirstName, entity.LastName, email);
    }

    public async Task<User?> CreateAsync(User user, string password)
    {
        var entity = new ApplicationUser
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.Email
        };

        var createResult = await userManager.CreateAsync(entity, password);

        return createResult.Succeeded ? new User(entity.Id, entity.FirstName, entity.LastName, user.Email) : null;
    }
}
