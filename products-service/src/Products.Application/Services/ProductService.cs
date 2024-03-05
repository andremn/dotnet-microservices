using FluentValidation;
using FluentValidation.Results;
using Products.Application.Services.Interfaces;
using Products.Application.Services.Results;
using Products.Domain.Models;
using Products.Domain.Repository;

namespace Products.Application.Services;

public class ProductService(
    IProductRepository productRepository,
    IValidator<Product> productValidator
) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator<Product> _productValidator = productValidator;

    public async Task<IList<Product>> GetAllAsync() =>
        await _productRepository.GetAllAsync();

    public async Task<Product?> FindByIdAsync(int id) =>
        await _productRepository.GetByIdAsync(id);

    public async Task<CreateProductResult> CreateAsync(Product product)
    {
        var validationResult = _productValidator.Validate(product);

        if (validationResult.IsValid)
        {
            product = await _productRepository.CreateAsync(product);

            return CreateProductResult.FromSuccess(product.Id);
        }

        return CreateProductResult.FromError(validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage));
    }

    public async Task<UpdateProductResult> UpdateAsync(Product product)
    {
        var validationResult = _productValidator.Validate(product);

        if (validationResult.IsValid)
        {
            var updatedProduct = await _productRepository.UpdateAsync(product);

            if (updatedProduct is null)
            {
                return UpdateProductResult.FromNotFoundError();
            }

            return UpdateProductResult.FromSuccess(updatedProduct);
        }

        return UpdateProductResult.FromValidationError(ConvertValidationFailureToDictionary(validationResult.Errors));
    }

    public async Task<UpdateProductResult> IncrementQuantityAsync(int id, int quantity)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
        {
            return UpdateProductResult.FromNotFoundError();
        }

        if (existingProduct.Quantity + quantity < 0)
        {
            var errors = new List<ValidationFailure>
            {
                new(nameof(quantity), $"'{nameof(quantity)}' to decrement cannot be greater than the current product quantity")
            };


            return UpdateProductResult.FromValidationError(ConvertValidationFailureToDictionary(errors));
        }

        await _productRepository.IncrementQuantityAsync(id, quantity);

        return UpdateProductResult.FromSuccess(existingProduct with { Quantity = existingProduct.Quantity + quantity });
    }

    public async Task<DeleteProductResult> DeleteByIdAsync(int id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
        {
            return DeleteProductResult.FromNotFoundError();
        }

        await _productRepository.DeleteAsync(existingProduct);

        return DeleteProductResult.FromSuccess();
    }

    private static Dictionary<string, string> ConvertValidationFailureToDictionary(List<ValidationFailure> validationFailures) =>
        validationFailures.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
}
