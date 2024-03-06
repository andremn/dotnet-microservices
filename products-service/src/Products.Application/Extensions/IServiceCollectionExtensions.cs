using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.Services;
using Products.Application.Services.Interfaces;
using Products.Application.Services.Validators;

namespace Products.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        services.AddValidatorsFromAssemblyContaining<ProductValidator>();

        return services;
    }
}
