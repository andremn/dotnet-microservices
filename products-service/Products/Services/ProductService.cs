using FluentValidation;
using FluentValidation.Results;
using Products.Model;
using Products.Repositories;
using Products.Services.Results;

namespace Products.Services;

public class ProductService(
    IProductRepository productRepository,
    IValidator<Product> productValidator
) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IValidator<Product> _productValidator = productValidator;

    public async Task<IList<Product>> GetAllAsync(IEnumerable<int> ids)
    {
        if (ids.Any())
        {
            return await _productRepository.GetAllByIdsAsync(ids);
        }

        return await _productRepository.GetAllAsync();
    }

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
        if (await _productRepository.GetByIdAsync(product.Id) == null)
        {
            return UpdateProductResult.FromNotFoundError();
        }

        var validationResult = _productValidator.Validate(product);

        if (validationResult.IsValid)
        {
            await _productRepository.UpdateAsync(product);

            return UpdateProductResult.FromSuccess();
        }

        return UpdateProductResult.FromValidationError(ConvertValidationFailureToDictionary(validationResult.Errors));
    }

    public async Task<UpdateProductResult> UpdateQuantityAsync(int id, int quantity)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
        {
            return UpdateProductResult.FromNotFoundError();
        }

        var productToUpdate = existingProduct with { Quantity = quantity };
        var validationResult = _productValidator.Validate(productToUpdate);

        if (validationResult.IsValid)
        {
            await _productRepository.UpdateAsync(productToUpdate);

            return UpdateProductResult.FromSuccess();
        }

        return UpdateProductResult.FromValidationError(ConvertValidationFailureToDictionary(validationResult.Errors));
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
