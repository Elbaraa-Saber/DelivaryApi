using BusinessLogicLayer.DTOs.Dish;
using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs.Order
{
    public class OrderDetailsDto : OrderListItemDto
    {
        public DateTime? DeliveryTime { get; set; }
        public List<DishListItemDto> Dishes { get; set; } = new();
    }
}
