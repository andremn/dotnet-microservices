using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Controllers.Products;
using Products.Model;
using Products.Services;
using Products.Services.Results;

namespace UnitTest.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly ProductsController _productsController;

    public ProductsControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _productsController = new ProductsController(_productServiceMock.Object);
    }

    [Fact]
    public async Task Get_NotEmpty_Returns200OkWithAllAvailableProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new(Id: 1, Name: "Keyboard", Description: "Keyboard with RGB lights", Quantity: 110, Price: 55.99m),
            new(Id: 2, Name: "Mouse", Description: "Mouse with Bluetooth connection", Quantity: 78, Price: 20.00m),
            new(Id: 3, Name: "Monitor", Description: "34\" 4K ultra-wide", Quantity: 9, Price: 399.90m)
        };

        _productServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productsController.Get();

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<IEnumerable<Product>>().Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task Get_Empty_Returns200OkWithEmptyList()
    {
        // Arrange
        _productServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync([]);

        // Act
        var result = await _productsController.Get();

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<IEnumerable<Product>>().Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_ExistingId_Returns200OkWithProductData()
    {
        // Arrange
        var products = new List<Product>
        {
            new(Id: 1, Name: "Keyboard", Description: "Keyboard with RGB lights", Quantity: 110, Price: 55.99m),
            new(Id: 2, Name: "Mouse", Description: "Mouse with Bluetooth connection", Quantity: 78, Price: 20.00m),
            new(Id: 3, Name: "Monitor", Description: "34\" 4K ultra-wide", Quantity: 9, Price: 399.90m)
        };

        var expectedProduct = products.Skip(1).First();

        _productServiceMock.Setup(x => x.FindByIdAsync(expectedProduct.Id)).ReturnsAsync(expectedProduct);

        // Act
        var result = await _productsController.GetById(expectedProduct.Id);

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<Product>().Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task GetById_NonExistingId_Returns404NotFound()
    {
        // Arrange
        var invalidId = 100;

        _productServiceMock.Setup(x => x.FindByIdAsync(invalidId)).ReturnsAsync((Product?)null);

        // Act
        var result = await _productsController.GetById(invalidId);

        // Assert
        result.Result.As<NotFoundResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Post_ValidProduct_Returns200OkWithProductId()
    {
        // Arrange
        var createdId = 12;
        var createProductRequest = new CreateProductRequest(Name: "CPU", Description: "Run all games", Quantity: 22, Price: 999.00m);
        var product = new Product(
            Id: 0,
            createProductRequest.Name,
            createProductRequest.Description,
            createProductRequest.Quantity,
            createProductRequest.Price);

        var expectedResponse = new CreateProductResponse(createdId);

        _productServiceMock.Setup(x => x.CreateAsync(product))
            .ReturnsAsync(CreateProductResult.FromSuccess(createdId));

        // Act
        var result = await _productsController.Post(createProductRequest);

        // Assert
        var createdAtResult = result.Result.As<CreatedAtActionResult>();

        createdAtResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdAtResult.Value.As<CreateProductResponse>().Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Post_InvalidProduct_Returns400BadRequestWithErrors()
    {
        // Arrange
        var createProductRequest = new CreateProductRequest(Name: "", Description: "", Quantity: -22, Price: -999.00m);

        var expectedErrors = new Dictionary<string, string>
        {
            ["name"]= "'Name' must be non-empty",
            ["price"] = "'Price' must be greater than or equal to 0",
            ["quantity"] = "'Quantity' must be greater than or equal to 0",
        };

        _productServiceMock.Setup(x => x.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(CreateProductResult.FromError(expectedErrors));

        // Act
        var result = await _productsController.Post(createProductRequest);

        // Assert
        var badRequestResult = result.Result.As<BadRequestObjectResult>();

        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.As<Dictionary<string, string>>().Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task Put_ExistingAndValidProduct_Returns200Ok()
    {
        // Arrange
        var productId = 52;
        var updateProductRequest = new UpdateProductRequest(Name: "CPU", Description: "Run all games", Quantity: 22, Price: 999.00m);
        var expectedProduct = new Product(
            productId,
            updateProductRequest.Name,
            updateProductRequest.Description,
            updateProductRequest.Quantity,
            updateProductRequest.Price);

        _productServiceMock.Setup(x => x.UpdateAsync(expectedProduct))
            .ReturnsAsync(new UpdateProductResult(true, expectedProduct, ResultErrorReason.None, new Dictionary<string, string>(0)));

        // Act
        var result = await _productsController.Put(productId, updateProductRequest);

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<Product>().Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task Put_NonExistingProduct_Returns404NotFound()
    {
        // Arrange
        var invalidId = 212;
        var updateProductRequest = new UpdateProductRequest(Name: "CPU", Description: "Run all games", Quantity: 22, Price: 999.00m);

        _productServiceMock.Setup(x => x.UpdateAsync(It.Is<Product>(y => y.Id == invalidId)))
            .ReturnsAsync(new UpdateProductResult(false, null, ResultErrorReason.NotFound, new Dictionary<string, string>(0)));

        // Act
        var result = await _productsController.Put(invalidId, updateProductRequest);

        // Assert
        result.Result.As<NotFoundResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Put_InvalidProduct_Returns400BadRequestWithErrors()
    {
        // Arrange
        var productId = 11;
        var updateProductRequest = new UpdateProductRequest(Name: "", Description: "", Quantity: -22, Price: -999.00m);

        var expectedErrors = new Dictionary<string, string>
        {
            ["name"] = "'Name' must be non-empty",
            ["price"] = "'Price' must be greater than or equal to 0",
            ["quantity"] = "'Quantity' must be greater than or equal to 0",
        };

        _productServiceMock.Setup(x => x.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(new UpdateProductResult(false, null, ResultErrorReason.Validation, expectedErrors));

        // Act
        var result = await _productsController.Put(productId, updateProductRequest);

        // Assert
        var badRequestResult = result.Result.As<BadRequestObjectResult>();

        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.As<Dictionary<string, string>>().Should().BeEquivalentTo(expectedErrors);
    }

    [Theory]
    [InlineData(UpdateProductQuantityOperation.Increment)]
    [InlineData(UpdateProductQuantityOperation.Decrement)]
    public async Task Patch_ExistingProduct_Returns200Ok(UpdateProductQuantityOperation operation)
    {
        // Arrange
        var productId = 52;
        var quantity = 151;
        var updateProductQuantityRequest = new UpdateProductQuantityRequest(quantity, operation.ToString());
        var expectedProduct = new Product(productId, Name: "CPU", Description: "Run all games", Quantity: 22, Price: 999.00m);
        var expectedQuantity = operation == UpdateProductQuantityOperation.Increment ? quantity : -quantity;

        _productServiceMock.Setup(x => x.IncrementQuantityAsync(productId, expectedQuantity))
            .ReturnsAsync(new UpdateProductResult(true, expectedProduct, ResultErrorReason.None, new Dictionary<string, string>(0)));

        // Act
        var result = await _productsController.Patch(productId, updateProductQuantityRequest);

        // Assert
        var okResult = result.Result.As<OkObjectResult>();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.As<Product>().Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task Patch_NonExistingProduct_Returns404NotFound()
    {
        // Arrange
        var invalidId = 212;
        var expectedQuantity = 151;
        var updateProductQuantityRequest = new UpdateProductQuantityRequest(expectedQuantity, UpdateProductQuantityOperation.Increment.ToString());
        var updateProductRequest = new UpdateProductRequest(Name: "CPU", Description: "Run all games", Quantity: 22, Price: 999.00m);

        _productServiceMock.Setup(x => x.IncrementQuantityAsync(invalidId, expectedQuantity))
            .ReturnsAsync(new UpdateProductResult(false, null, ResultErrorReason.NotFound, new Dictionary<string, string>(0)));

        // Act
        var result = await _productsController.Patch(invalidId, updateProductQuantityRequest);

        // Assert
        result.Result.As<NotFoundResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Patch_InvalidProduct_Returns400BadRequestWithErrors()
    {
        // Arrange
        var productId = 11;
        var expectedQuantity = 151;
        var updateProductQuantityRequest = new UpdateProductQuantityRequest(expectedQuantity, UpdateProductQuantityOperation.Increment.ToString());
        var updateProductRequest = new UpdateProductRequest(Name: "", Description: "", Quantity: -22, Price: -999.00m);

        var expectedErrors = new Dictionary<string, string>
        {
            ["quantity"] = "'Quantity' must be greater than or equal to 0",
        };

        _productServiceMock.Setup(x => x.IncrementQuantityAsync(productId, expectedQuantity))
            .ReturnsAsync(new UpdateProductResult(false, null, ResultErrorReason.Validation, expectedErrors));

        // Act
        var result = await _productsController.Patch(productId, updateProductQuantityRequest);

        // Assert
        var badRequestResult = result.Result.As<BadRequestObjectResult>();

        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.As<Dictionary<string, string>>().Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task Patch_InvalidOperation_Returns400BadRequestWithErrors()
    {
        // Arrange
        var productId = 11;
        var expectedQuantity = 151;
        var updateProductQuantityRequest = new UpdateProductQuantityRequest(expectedQuantity, "invalid");
        var updateProductRequest = new UpdateProductRequest(Name: "", Description: "", Quantity: -22, Price: -999.00m);

        var expectedErrors = new Dictionary<string, string>
        {
            ["Operation"] = $"Invalid operation '{updateProductQuantityRequest.Operation}'",
        };

        _productServiceMock.Setup(x => x.IncrementQuantityAsync(productId, expectedQuantity))
            .ReturnsAsync(new UpdateProductResult(false, null, ResultErrorReason.Validation, expectedErrors));

        // Act
        var result = await _productsController.Patch(productId, updateProductQuantityRequest);

        // Assert
        var badRequestResult = result.Result.As<BadRequestObjectResult>();

        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.As<Dictionary<string, string>>().Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task Delete_ExistingProduct_Returns204NoContent()
    {
        // Arrange
        var idToDelete = 15;

        _productServiceMock.Setup(x => x.DeleteByIdAsync(idToDelete))
            .ReturnsAsync(new DeleteProductResult(true));

        // Act
        var result = await _productsController.Delete(idToDelete);

        // Assert
        result.As<NoContentResult>().StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task Delete_NonExistingProduct_Returns404NotFound()
    {
        // Arrange
        var invalidId = 212;

        _productServiceMock.Setup(x => x.DeleteByIdAsync(invalidId))
            .ReturnsAsync(new DeleteProductResult(false));

        // Act
        var result = await _productsController.Delete(invalidId);

        // Assert
        result.As<NotFoundResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}