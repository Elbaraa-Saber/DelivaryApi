using BusinessLogicLayer.DTOs.Order;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET /api/order
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] OrderQueryParamsDto query)
        {
            var result = await _orderService.GetOrdersAsync(query);
            return Ok(result);
        }

        // GET /api/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(Guid id)
        {
            var result = await _orderService.GetOrderDetailsAsync(id);
            return Ok(result);
        }
    }
}
