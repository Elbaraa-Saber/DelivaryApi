using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Cart;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DishBasketDto>> GetUserCartAsync(Guid userId)
        {
            return await _context.DishInCarts
                .Include(c => c.Dish)
                .Where(c => c.UserId == userId && c.OrderId == null)
                .Select(c => new DishBasketDto
                {
                    DishId = c.DishId,
                    Name = c.Dish.Name,
                    Price = c.Dish.Price,
                    Count = c.Count
                })
                .ToListAsync();
        }

        public async Task AddDishToCartAsync(Guid userId, Guid dishId)
        {
            var cartItem = await _context.DishInCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.DishId == dishId && c.OrderId == null);

            if (cartItem == null)
            {
                cartItem = new DishInCart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DishId = dishId,
                    Count = 1,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = DateTime.UtcNow
                };
                await _context.DishInCarts.AddAsync(cartItem);
            }
            else
            {
                cartItem.Count++;
                cartItem.ModifyDateTime = DateTime.UtcNow;
                _context.DishInCarts.Update(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveDishFromCartAsync(Guid userId, Guid dishId, bool decreaseOnly = false)
        {
            var cartItem = await _context.DishInCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.DishId == dishId && c.OrderId == null);

            if (cartItem == null)
                return;

            if (decreaseOnly && cartItem.Count > 1)
            {
                cartItem.Count--;
                cartItem.ModifyDateTime = DateTime.UtcNow;
                _context.DishInCarts.Update(cartItem);
            }
            else
            {
                _context.DishInCarts.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}
