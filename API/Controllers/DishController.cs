using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
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
    }
}
