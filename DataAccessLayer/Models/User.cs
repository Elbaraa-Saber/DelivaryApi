using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Models
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }

        // from IBaseEntity
        public Guid Id { get; set; } 
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }

        // relations with other Tables
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<DishInCart> CartItems { get; set; } = new List<DishInCart>();
    }
}
