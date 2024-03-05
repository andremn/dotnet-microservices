using Microsoft.AspNetCore.Identity;
using Products.Infrastructure.Identity;
using Products.Domain.Repository;
using Products.Domain.Models;

namespace Products.Infrastructure.Data.Repositories;

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

    public async Task<bool> CreateAsync(User dto, string password)
    {
        var entity = new ApplicationUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email
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
