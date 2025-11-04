namespace BusinessLogicLayer.DTOs.Cart
{
    public class DishBasketDto
    {
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice => Price * Count;
    }
}
