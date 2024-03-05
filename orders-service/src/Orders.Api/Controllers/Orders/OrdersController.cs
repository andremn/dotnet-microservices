using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Services.Interfaces;
using Orders.Domain.Models;

namespace Orders.Controllers.Orders;

/// <summary>
/// Orders controller.
/// </summary>
/// <param name="orderService">The <see cref="IOrderService"/> to handle order requests.</param>
/// <response code="401">If the request is not authenticated.</response>
[Route("api/orders")]
[Authorize]
[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    /// <summary>
    /// Gets all available orders for the logged user.
    /// </summary>
    /// <returns>The orders for the logged user.</returns>
    /// <response code="200">Returns the available orders for the logged user.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<Order>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Order>> Get()
    {
        return Ok(await orderService.GetAllAsync());
    }

    /// <summary>
    /// Gets an order with the specified id, if any.
    /// </summary>
    /// <param name="id">The id of the order.</param>
    /// <returns>The order with the specified id.</returns>
    /// <response code="200">Returns the order with the specified id.</response>
    /// <response code="404">If no order with the specified id was found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> GetById([FromRoute] int id)
    {
        var order = await orderService.GetByIdAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    /// <summary>
    /// Creates an order with the specified data.
    /// </summary>
    /// <param name="request">The data to create the order.</param>
    /// <returns>The id of the created order.</returns>
    /// <response code="201">Returns the id of the newly created order.</response>
    /// <response code="404">If the provided product does not exist or the quantity is not enough for this order request.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status201Created)]
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
