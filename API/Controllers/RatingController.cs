using BusinessLogicLayer.DTOs.Rating;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/dish/{dishId}/rating")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet("check")]
        public async Task<IActionResult> CheckIfUserCanRate(Guid dishId)
        {
            var canRate = await _ratingService.CanUserRateDishAsync(CurrentUserId, dishId);
            return Ok(new { canRate });
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRating(Guid dishId, [FromBody] RatingDto ratingDto)
        {
            await _ratingService.AddOrUpdateRatingAsync(CurrentUserId, dishId, ratingDto);
            return Ok(new { message = "Rating added/updated successfully" });
        }
    }
}
