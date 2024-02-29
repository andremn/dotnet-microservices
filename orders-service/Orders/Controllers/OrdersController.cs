using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Services;

namespace Orders.Controllers;

[Route("api/orders")]
[Authorize]
[ApiController]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok(await orderService.GetAllAsync());
    }
}
