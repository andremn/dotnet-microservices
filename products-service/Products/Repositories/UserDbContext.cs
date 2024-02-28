using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Products.Repositories.Entities;

namespace Products.Repositories;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<UserEntity>(options)
{
}
