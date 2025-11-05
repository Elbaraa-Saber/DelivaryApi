using BusinessLogicLayer.DTOs.Dish;
using BusinessLogicLayer.DTOs.Rating;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        private readonly IRatingService _ratingService;

        public DishController(IDishService dishService, IRatingService ratingService)
        {
            _dishService = dishService;
            _ratingService = ratingService;
        }

        // GET /api/dish
        [HttpGet]
        public async Task<IActionResult> GetDishes([FromQuery] DishQueryParamsDto query)
        {
            var result = await _dishService.GetDishesAsync(query);
            return Ok(result);
        }

        // GET /api/dish/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishDetails(Guid id)
        {
            var result = await _dishService.GetDishDetailsAsync(id);
            return Ok(result);
        }

        // GET /api/dish/{id}/rating/check
        [Authorize]
        [HttpGet("{id}/rating/check")]
        public async Task<IActionResult> CheckCanRate(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var canRate = await _ratingService.CanUserRateDishAsync(userId, id);
            return Ok(canRate);
        }

        // POST /api/dish/{id}/rating
        [Authorize]
        [HttpPost("{id}/rating")]
        public async Task<IActionResult> SetRating(Guid id, [FromQuery] int ratingScore)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _ratingService.AddOrUpdateRatingAsync(userId, id, new RatingDto { Value = ratingScore });
            return Ok(new { message = "Rating set successfully" });
        }
    }
}
