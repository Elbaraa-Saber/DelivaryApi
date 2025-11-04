using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ICartService _cartService;

        public BasketController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

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
            return Ok(new { message = "Dish added to your cart." });
        }

        [HttpDelete("dish/{dishId}")]
        public async Task<IActionResult> RemoveDishFromCart(Guid dishId, [FromQuery] bool decreaseOnly = false)
        {
            await _cartService.RemoveDishFromCartAsync(CurrentUserId, dishId, decreaseOnly);
            return Ok(new { message = "Dish removed or decreased from your cart." });
        }
    }
}
