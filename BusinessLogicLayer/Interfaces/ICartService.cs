using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Cart;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<DishBasketDto>> GetUserCartAsync(Guid userId);
        Task AddDishToCartAsync(Guid userId, Guid dishId);
        Task RemoveDishFromCartAsync(Guid userId, Guid dishId, bool decreaseOnly = false);
    }
}

