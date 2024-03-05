using Microsoft.AspNetCore.Identity;
using Users.Domain.Models;
using Users.Domain.Repository;
using Users.Infrastructure.Identity;

namespace Users.Infrastructure.Data.Repositories;

internal class UserRepository(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager
) : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

        return result.Succeeded ? new User(user.Id, user.FirstName, user.LastName, email) : null;
    }

    public async Task<bool> CreateAsync(User user, string password)
    {
        var entity = new ApplicationUser
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.Email
        };

        var createResult = await _userManager.CreateAsync(entity, password);

        return createResult.Succeeded;
    }
}
