using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Products.Model;
using Products.Repositories;
using Products.Services;
using Products.Services.Results;

namespace UnitTest.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IValidator<Product>> _productValidatorMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productValidatorMock = new Mock<IValidator<Product>>();
        _productService = new ProductService(
            _productRepositoryMock.Object,
            _productValidatorMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_NonEmptyRepository_ReturnsAllAvailableProducts()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new(Id: 1, Name: "Keyboard", Description: "Keyboard with RGB lights", Quantity: 110, Price: 55.99m),
            new(Id: 2, Name: "Mouse", Description: "Mouse with Bluetooth connection", Quantity: 78, Price: 20.00m),
            new(Id: 3, Name: "Monitor", Description: "34\" 4K ultra-wide", Quantity: 9, Price: 399.90m)
        };

        _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expectedProducts);

        // Act
        var actualProducts = await _productService.GetAllAsync();

        // Assert
        actualProducts.Should().BeEquivalentTo(expectedProducts);
    }

    [Fact]
    public async Task GetAllAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);

        // Act
        var actualProducts = await _productService.GetAllAsync();

        // Assert
        actualProducts.Should().BeEmpty();
    }

    [Fact]
    public async Task FindByIdAsync_ProductWithIdExists_ReturnsProductWithMatchingId()
    {
        // Arrange
        var existingProducts = new List<Product>
        {
            new(Id: 1, Name: "Keyboard", Description: "Keyboard with RGB lights", Quantity: 110, Price: 55.99m),
            new(Id: 2, Name: "Mouse", Description: "Mouse with Bluetooth connection", Quantity: 78, Price: 20.00m),
            new(Id: 3, Name: "Monitor", Description: "34\" 4K ultra-wide", Quantity: 9, Price: 399.90m)
        };

        var expectedProduct = existingProducts.First();

        _productRepositoryMock.Setup(x => x.GetByIdAsync(expectedProduct.Id)).ReturnsAsync(expectedProduct);

        // Act
        var actualProduct = await _productService.FindByIdAsync(expectedProduct.Id);

        // Assert
        actualProduct.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task FindByIdAsync_ProductWithNonExistingId_ReturnsNull()
    {
        // Arrange
        var invalidId = 100;
        var existingProducts = new List<Product>
        {
            new(Id: 1, Name: "Keyboard", Description: "Keyboard with RGB lights", Quantity: 110, Price: 55.99m),
            new(Id: 2, Name: "Mouse", Description: "Mouse with Bluetooth connection", Quantity: 78, Price: 20.00m),
            new(Id: 3, Name: "Monitor", Description: "34\" 4K ultra-wide", Quantity: 9, Price: 399.90m)
        };

        _productRepositoryMock.Setup(x => x.GetByIdAsync(invalidId)).ReturnsAsync((Product?)null);

        // Act
        var actualProduct = await _productService.FindByIdAsync(invalidId);

        // Assert
        actualProduct.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ValidProduct_ReturnsSuccessWithProductData()
    {
        // Arrange
        var createdId = 121;
        var productToCreate = new Product(
            Id: 0,
            Name: "Keyboard",
            Description: "Keyboard with RGB lights",
            Quantity: 110,
            Price: 55.99m);

        _productRepositoryMock.Setup(x => x.CreateAsync(productToCreate))
            .ReturnsAsync(productToCreate with { Id = createdId });

        var expectedResult = CreateProductResult.FromSuccess(createdId);

        _productValidatorMock.Setup(x => x.Validate(productToCreate)).Returns(new ValidationResult());

        // Act
        var actualResult = await _productService.CreateAsync(productToCreate);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateAsync_InvalidProduct_ReturnsSuccessWithProductData()
    {
        // Arrange
        var productToCreate = new Product(
            Id: 0,
            Name: "",
            Description: "Keyboard with RGB lights",
            Quantity: -110,
            Price: -55.99m);

        var expectedErrors = new Dictionary<string, string>
        {
            ["name"] = "'Name' must be non-empty",
            ["price"] = "'Price' must be greater than or equal to 0",
            ["quantity"] = "'Quantity' must be greater than or equal to 0",
        };

        var validationResult = new ValidationResult(expectedErrors.Select(x => new ValidationFailure(x.Key, x.Value)));

        var expectedResult = CreateProductResult.FromError(expectedErrors);

        _productValidatorMock.Setup(x => x.Validate(productToCreate)).Returns(validationResult);

        // Act
        var actualResult = await _productService.CreateAsync(productToCreate);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}
