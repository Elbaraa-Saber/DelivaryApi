using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs.Dish
{
    public class DishDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }        
        public bool Vegetarian { get; set; }      
        public double Rating { get; set; }       
        public Category Category { get; set; }   
    }
}
