using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs
{
    public class DishListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<Category> Categories { get; set; } = new();
        public string Photo { get; set; }
    }
}
