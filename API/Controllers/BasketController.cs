using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ICartService _cartService;

        public BasketController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // مؤقتًا — حتى نضيف نظام المستخدمين (JWT)
        private Guid CurrentUserId => Guid.Parse("11111111-1111-1111-1111-111111111111");

        [HttpGet]
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartService.GetUserCartAsync(CurrentUserId);
            return Ok(cart);
        }

        [HttpPost("dish/{dishId}")]
        public async Task<IActionResult> AddDishToCart(Guid dishId)
        {
            await _cartService.AddDishToCartAsync(CurrentUserId, dishId);
            return Ok(new { message = "Dish added to cart." });
        }

        [HttpDelete("dish/{dishId}")]
        public async Task<IActionResult> RemoveDishFromCart(Guid dishId, [FromQuery] bool decreaseOnly = false)
        {
            await _cartService.RemoveDishFromCartAsync(CurrentUserId, dishId, decreaseOnly);
            return Ok(new { message = "Dish removed or decreased." });
        }
    }
}
