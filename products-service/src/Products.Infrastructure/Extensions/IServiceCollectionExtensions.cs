using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Products.Infrastructure.Data;
using Products.Infrastructure.Data.Repositories;
using Products.Infrastructure.Identity;
using Products.Domain.Repository;

namespace Products.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        }).AddEntityFrameworkStores<UsersDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddProductsDbContext(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<ProductsDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }

    public static IServiceCollection AddUsersDbContext(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<UsersDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}
