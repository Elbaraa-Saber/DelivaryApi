using BusinessLogicLayer.DTOs.Rating;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext _context;

        public RatingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CanUserRateDishAsync(Guid userId, Guid dishId)
        {
            // المستخدم يمكنه تقييم الطبق فقط إن كان قد طلبه (OrderId != null)
            var hasOrderedDish = await _context.DishInCarts
                .AnyAsync(c => c.UserId == userId && c.DishId == dishId && c.OrderId != null);

            return hasOrderedDish;
        }

        public async Task AddOrUpdateRatingAsync(Guid userId, Guid dishId, RatingDto ratingDto)
        {
            var existing = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.DishId == dishId);

            if (existing == null)
            {
                var newRating = new Rating
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DishId = dishId,
                    Value = ratingDto.Value,
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = DateTime.UtcNow
                };
                await _context.Ratings.AddAsync(newRating);
            }
            else
            {
                existing.Value = ratingDto.Value;
                existing.ModifyDateTime = DateTime.UtcNow;
                _context.Ratings.Update(existing);
            }

            await _context.SaveChangesAsync();
        }
    }
}
