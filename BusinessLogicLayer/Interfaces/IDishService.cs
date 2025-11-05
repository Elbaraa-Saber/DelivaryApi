using BusinessLogicLayer.DTOs.Dish;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDishService
    {
        Task<DishListResponseDto> GetDishesAsync(DishQueryParamsDto query);
        Task<DishDetailsDto> GetDishDetailsAsync(Guid id);
    }
}
