using BusinessLogicLayer.DTOs.Common;
using BusinessLogicLayer.DTOs.Dish;

namespace BusinessLogicLayer.Interfaces
{
    public interface IDishService
    {
        Task<PagedResult<DishListItemDto>> GetDishesAsync(DishQueryParamsDto query);
        Task<DishDetailsDto> GetDishDetailsAsync(Guid id);
    }
}
