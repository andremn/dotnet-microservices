using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Products.Repositories.Entities;

namespace Products.Repositories;

public class UserDbContext(DbContextOptions options) : IdentityDbContext<UserEntity>(options)
{
}
