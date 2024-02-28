using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Extensions;
using Products.Model;
using Products.Services;
using Products.Services.Results;

namespace Products.Controllers.Products;

[Route("api/products")]
[Authorize]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> Get() =>
        Ok(await _productService.GetAllAsync());

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

    [HttpPost]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateProductResponse>> Post([FromBody] CreateProductRequest request)
    {
        var result = await _productService.CreateAsync(request.ToModel());

        if (result.Success)
        {
            var response = new CreateProductResponse(result.Id);

            return CreatedAtAction(nameof(GetById), response, response);
        }

        return BadRequest(result.Errors);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put([FromBody] UpdateProductRequest request)
    {
        var result = await _productService.UpdateAsync(request.ToModel());

        if (result.Success)
        {
            return NoContent();
        }

        return result.ErrorReason == ResultErrorReason.NotFound ? NotFound() : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var result = await _productService.DeleteByIdAsync(id);

        return result.Success ? NoContent() : NotFound();
    }
}
