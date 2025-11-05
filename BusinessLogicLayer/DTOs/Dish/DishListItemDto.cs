using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs.Dish
{
    public class DishListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Category> Categories { get; set; } = new();
        public string Photo { get; set; }
        public string Description { get; internal set; }
        public bool IsVegetarian { get; internal set; }
        public double AverageRating { get; internal set; }
    }
}
