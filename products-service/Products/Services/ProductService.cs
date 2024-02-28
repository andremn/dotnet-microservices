using FluentValidation;
using Products.Model;
using Products.Repositories;
using Products.Services.Results;

namespace Products.Services;

public class ProductService(IProductRepository productRepository, IValidator<Product> productValidator) : IProductService
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

        return CreateProductResult.FromError(
            validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage));
    }
}
