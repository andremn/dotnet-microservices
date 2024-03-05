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

    public async Task<bool> LoginAsync(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

        return result.Succeeded;
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

    public async Task<User?> FindByEmailAsync(string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            return new User(existingUser.Id, existingUser.FirstName, existingUser.LastName, email);
        }

        return null;
    }
}
