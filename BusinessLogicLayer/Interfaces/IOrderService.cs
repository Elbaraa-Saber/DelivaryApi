using BusinessLogicLayer.DTOs.Common;
using BusinessLogicLayer.DTOs.Order;

namespace BusinessLogicLayer.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResult<OrderListItemDto>> GetOrdersAsync(OrderQueryParamsDto query);
        Task<OrderDetailsDto> GetOrderDetailsAsync(Guid id);
    }
}
