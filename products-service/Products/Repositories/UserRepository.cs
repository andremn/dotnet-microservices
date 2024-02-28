using Microsoft.AspNetCore.Identity;
using Products.Model;
using Products.Repositories.Entities;

namespace Products.Repositories;

public class UserRepository(
    UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager
) : IUserRepository
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<bool> LoginAsync(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

        return result.Succeeded;
    }

    public async Task<bool> CreateAsync(User user, string password)
    {
        var entity = new UserEntity
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
