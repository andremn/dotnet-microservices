using Microsoft.AspNetCore.Identity;

namespace Products.Repositories.Entities;

public class UserEntity : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}
