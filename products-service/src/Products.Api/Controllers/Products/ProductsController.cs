using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Mapping;
using Products.Application.Enums;
using Products.Application.Services.Interfaces;
using Products.Domain.Models;

namespace Products.Api.Controllers.Products;

/// <summary>
/// Products controller.
/// </summary>
/// <param name="productService">The <see cref="IProductService"/> to handle product requests.</param>
/// <response code="401">If the request is not authenticated.</response>
[Route("api/products")]
[Authorize]
[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    /// <summary>
    /// Gets all available products.
    /// </summary>
    /// <returns>All products available.</returns>
    /// <response code="200">Returns the available products.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> Get() =>
        Ok(await _productService.GetAllAsync());

    /// <summary>
    /// Gets a product with the specified id, if any.
    /// </summary>
    /// <param name="id">The id of the product.</param>
    /// <returns>The product with the specified id.</returns>
    /// <response code="200">Returns the product with the specified id.</response>
    /// <response code="404">If no product with the specified id was found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetById([FromRoute] int id)
    {
        var product = await _productService.FindByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Creates a product with the specified data.
    /// </summary>
    /// <param name="request">The data to create the product.</param>
    /// <returns>The id of the created product.</returns>
    /// <response code="201">Returns the id of the newly created product.</response>
    /// <response code="400">If the provided data to create the product is not valid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateProductResponse>> Post([FromBody] CreateProductRequest request)
    {
        var result = await _productService.CreateAsync(request.ToDto());

        if (result.Success)
        {
            var response = new CreateProductResponse(result.Id);

            return CreatedAtAction(nameof(GetById), response, response);
        }

        return BadRequest(result.Errors);
    }

    /// <summary>
    /// Updates a product with the given id with the provided data.
    /// </summary>
    /// <param name="id">The id of the product to update.</param>
    /// <param name="request">The data to update the product.</param>
    /// <returns>The updated product.</returns>
    /// <response code="200">Returns the updated product.</response>
    /// <response code="400">If the provided data to update the product is not valid.</response>
    /// <response code="404">If no product with the specified id was found.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> Put([FromRoute] int id, [FromBody] UpdateProductRequest request)
    {
        var result = await _productService.UpdateAsync(request.ToDto(id));

        if (result.Success)
        {
            return Ok(result.Product);
        }

        return result.ErrorReason == ResultErrorReason.NotFound ? NotFound() : BadRequest(result.Errors);
    }

    /// <summary>
    /// Updates the quantity of a product with the given id.
    /// </summary>
    /// <param name="id">The id of the product to update.</param>
    /// <param name="request">The data containing the operation and quantity value.</param>
    /// <returns>The updated product.</returns>
    /// <response code="200">Returns the updated product.</response>
    /// <response code="400">If the provided data to update the product quantity is not valid.</response>
    /// <response code="404">If no product with the specified id was found.</response>
    [HttpPatch("{id}/quantity")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> Patch([FromRoute] int id, [FromBody] UpdateProductQuantityRequest request)
    {
        if (Enum.TryParse<UpdateProductQuantityOperation>(request.Operation, ignoreCase: true, out var operation))
        {
            var quantity = operation == UpdateProductQuantityOperation.Increment ? request.Quantity : -request.Quantity;
            var result = await _productService.IncrementQuantityAsync(id, quantity);

            if (result.Success)
            {
                return Ok(result.Product);
            }

            return result.ErrorReason == ResultErrorReason.NotFound ? NotFound() : BadRequest(result.Errors);
        }

        return BadRequest(new Dictionary<string, string>
        {
            [nameof(request.Operation)] = $"Invalid operation '{request.Operation}'"
        });
    }

    /// <summary>
    /// Deletes a product with the given id.
    /// </summary>
    /// <param name="id">The id of the product to delete.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the delete was successful.</response>
    /// <response code="404">If no product with the specified id was found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var result = await _productService.DeleteByIdAsync(id);

        return result.Success ? NoContent() : NotFound();
    }
}
