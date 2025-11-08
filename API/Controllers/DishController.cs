using BusinessLogicLayer.DTOs.Dish;
using BusinessLogicLayer.DTOs.Rating;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.DTOs.Common;
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
        [ProducesResponseType(typeof(DishListResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> GetDishes([FromQuery] DishQueryParamsDto query)
        {
            try
            {
                var result = await _dishService.GetDishesAsync(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponseDto
                {
                    Status = "Bad Request",
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDto
                {
                    Status = "InternalServerError",
                    Message = ex.Message
                });
            }
        }

        // GET /api/dish/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DishDetailsDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 404)]
        [ProducesResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> GetDishDetails(Guid id)
        {
            try
            {
                var result = await _dishService.GetDishDetailsAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponseDto
                {
                    Status = "Not Found",
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDto
                {
                    Status = "InternalServerError",
                    Message = ex.Message
                });
            }
        }


        // GET /api/dish/{id}/rating/check
        [Authorize]
        [HttpGet("{id}/rating/check")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(ErrorResponseDto), 404)]
        [ProducesResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> CheckCanRate(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var canRate = await _ratingService.CanUserRateDishAsync(userId, id);
                return Ok(canRate); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponseDto
                {
                    Status = "Not Found",
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDto
                {
                    Status = "InternalServerError",
                    Message = ex.Message
                });
            }
        }


        // POST /api/dish/{id}/rating
        [Authorize]
        [HttpPost("{id}/rating")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(ErrorResponseDto), 404)]
        [ProducesResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> SetRating(Guid id, [FromQuery] int ratingScore)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (ratingScore < 1 || ratingScore > 5)
                {
                    return BadRequest(); 
                }

                await _ratingService.AddOrUpdateRatingAsync(userId, id, new RatingDto { Value = ratingScore });
                return Ok(); 
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponseDto
                {
                    Status = "Not Found",
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDto
                {
                    Status = "InternalServerError",
                    Message = ex.Message
                });
            }
        }

    }
}
