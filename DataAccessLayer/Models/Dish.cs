using DataAccessLayer.Common;

namespace DataAccessLayer.Models
{
    public class Dish : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsVegetarian { get; set; }
        public string Photo { get; set; }

        // Relations
        public ICollection<DishCategory> DishCategories { get; set; } = new List<DishCategory>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<DishInCart> DishInCarts { get; set; } = new List<DishInCart>();

        // IBaseEntity
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
    }
}
