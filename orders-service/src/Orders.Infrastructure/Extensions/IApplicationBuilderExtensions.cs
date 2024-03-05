using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Infrastructure.Data;

namespace Orders.Infrastructure.Extensions;

public static class IApplicationBuilderExtensions
{
    public static void UseExecuteMigrations(this IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<OrdersDbContext>();
        var migrationsToApply = context.Database.GetPendingMigrations();

        Console.WriteLine("Pending migrations = {0}", string.Join(",", migrationsToApply));

        context.Database.Migrate();

        Console.WriteLine("Migrations applied successfully");
    }
}
