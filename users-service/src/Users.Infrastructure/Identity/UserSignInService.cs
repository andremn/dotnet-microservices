using Microsoft.AspNetCore.Identity;
using Users.Application.Services;

namespace Users.Infrastructure.Identity;

public class UserSignInService(SignInManager<ApplicationUser> signInManager) : IUserSignInService
{
    public async Task<bool> SignInAsync(string email, string password)
    {
        var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: false, 
            lockoutOnFailure: false);

        return result.Succeeded;
    }
}