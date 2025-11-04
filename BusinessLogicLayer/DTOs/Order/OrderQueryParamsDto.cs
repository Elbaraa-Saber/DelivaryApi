using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs.Order
{
    public class OrderQueryParamsDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Status? Status { get; set; }
        public Guid? UserId { get; set; } 
    }
}
