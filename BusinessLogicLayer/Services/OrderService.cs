using BusinessLogicLayer.DTOs.Common;
using BusinessLogicLayer.DTOs.Order;
using BusinessLogicLayer.DTOs.Dish;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using BusinessLogicLayer.Interfaces;


namespace BusinessLogicLayer.Services
{

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<OrderListItemDto>> GetOrdersAsync(OrderQueryParamsDto query)
        {
            var orders = _context.Orders.AsQueryable();

            if (query.Status.HasValue)
                orders = orders.Where(o => o.Status == query.Status.Value);

            if (query.UserId.HasValue)
                orders = orders.Where(o => o.UserId == query.UserId.Value);

            var total = await orders.CountAsync();

            var items = await orders
                .OrderByDescending(o => o.OrderTime)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(o => new OrderListItemDto
                {
                    Id = o.Id,
                    OrderTime = o.OrderTime,
                    Price = o.Price,
                    Status = o.Status,
                    Address = o.Address
                })
                .ToListAsync();

            return new PagedResult<OrderListItemDto>
            {
                Items = items,
                TotalCount = total,
                CurrentPage = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<OrderDetailsDto> GetOrderDetailsAsync(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.DishInCarts)
                .ThenInclude(dic => dic.Dish)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new KeyNotFoundException($"Order with id {id} not found.");

            return new OrderDetailsDto
            {
                Id = order.Id,
                OrderTime = order.OrderTime,
                DeliveryTime = order.DeliveryTime,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status,
                Dishes = order.DishInCarts.Select(dic => new DishListItemDto
                {
                    Id = dic.Dish.Id,
                    Name = dic.Dish.Name,
                    Price = dic.Dish.Price,
                    Photo = dic.Dish.Photo
                }).ToList()
            };
        }
    }
}
