using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Model;
using Orders.Services.Orders;

namespace Orders.Controllers.Orders;

[Route("api/orders")]
[Authorize]
[ApiController]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IList<Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Order>> Get()
    {
        return Ok(await orderService.GetAllAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public async Task<ActionResult<Order>> GetById([FromRoute] int id)
    {
        var order = await orderService.GetByIdAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateOrderResponse>> Post([FromBody] CreateOrderRequest request)
    {
        var result = await orderService.CreateAsync(request.ProductId, request.Quantity);

        if (result.IsSuccess)
        {
            var response = new CreateOrderResponse(result.Id);

            return CreatedAtAction(nameof(GetById), response, response);
        }

        return NotFound();
    }
}
