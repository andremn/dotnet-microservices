using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Model;
using Orders.Services;

namespace Orders.Controllers.Orders;

[Route("api/orders")]
[Authorize]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IList<DetailedOrder>), StatusCodes.Status200OK)]
    public async Task<ActionResult<DetailedOrder>> Get()
    {
        return Ok(await orderService.GetAllAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DetailedOrder), StatusCodes.Status200OK)]
    public async Task<ActionResult<DetailedOrder>> GetById([FromRoute] int id)
    {
        var result = await orderService.GetByIdAsync(id);

        if (result.Order is null)
        {
            return NotFound();
        }

        return Ok(result.Order);
    }

    [HttpPost]
    [ProducesResponseType(typeof(IList<DetailedOrder>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Post([FromBody] PostRequest request)
    {
        var result = await orderService.CreateAsync(request.ProductId, request.Quantity);

        if (result.IsSuccess)
        {
            var response = new PostResponse(result.Id);

            return CreatedAtAction(nameof(GetById), response, response);
        }

        return NotFound();
    }
}
