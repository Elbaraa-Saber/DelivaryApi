using BusinessLogicLayer.DTOs.Order;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // 1. GET /api/order
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var query = new OrderQueryParamsDto
            {
                UserId = CurrentUserId,
                Page = 1,
                PageSize = 50
            };

            var orders = await _orderService.GetOrdersAsync(query);
            return Ok(orders.Items);
        }

        // 2. GET /api/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderDetailsAsync(id);

            if (order == null)
                return NotFound(new { status = "NotFound", message = "Order not found" });

            return Ok(order);
        }

        // 3. POST /api/order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Address))
                return BadRequest(new { status = "BadRequest", message = "Address is required" });

            var orderId = await _orderService.CreateOrderFromCartAsync(CurrentUserId, dto.Address);
            return Ok(new { id = orderId, message = "Order created successfully" });
        }

        // 4. POST /api/order/{id}/status
        [HttpPost("{id}/status")]
        public async Task<IActionResult> ConfirmOrderStatus(Guid id)
        {
            await _orderService.ConfirmOrderDeliveryAsync(id, CurrentUserId);
            return Ok(new { message = "Order confirmed as delivered" });
        }
    }

    public class CreateOrderDto
    {
        public DateTime DeliveryTime { get; set; } = DateTime.UtcNow.AddHours(1);
        public string Address { get; set; }
    }
}
