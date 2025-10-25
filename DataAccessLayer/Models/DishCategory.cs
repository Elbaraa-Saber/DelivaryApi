using DataAccessLayer.Common;

namespace DataAccessLayer.Models
{
    public class DishCategory
    {
        public Guid DishId { get; set; }
        public Dish Dish { get; set; }

        public Category Category { get; set; }
    }
}
