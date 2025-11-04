using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs.Dish
{
    public class DishDetailsDto : DishListItemDto
    {
        public string Description { get; set; }
        public bool IsVegetarian { get; set; }
        public double AverageRating { get; set; }
    }
}
