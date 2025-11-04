using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs.Order
{
    public class OrderListItemDto
    {
        public Guid Id { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal Price { get; set; }
        public Status Status { get; set; }
        public string Address { get; set; }
    }
}
