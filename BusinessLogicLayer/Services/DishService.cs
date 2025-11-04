using BusinessLogicLayer.DTOs.Common;
using BusinessLogicLayer.DTOs.Dish;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Common;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{

    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DishListItemDto>> GetDishesAsync(DishQueryParamsDto query)
        {
            var dishes = _context.Dishes
                .Include(d => d.DishCategories)
                .AsQueryable();

            if (query.Categories != null && query.Categories.Any())
            {
                dishes = dishes.Where(d =>
                    d.DishCategories.Any(dc => query.Categories.Contains(dc.Category)));
            }

            if (!string.IsNullOrEmpty(query.Search))
            {
                dishes = dishes.Where(d => d.Name.ToLower().Contains(query.Search.ToLower()));
            }

            if (query.IsVegetarian.HasValue)
            {
                dishes = dishes.Where(d => d.IsVegetarian == query.IsVegetarian.Value);
            }

            var total = await dishes.CountAsync();

            var items = await dishes
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(d => new DishListItemDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Photo = d.Photo,
                    Categories = d.DishCategories.Select(dc => dc.Category).ToList()
                })
                .ToListAsync();

            return new PagedResult<DishListItemDto>
            {
                Items = items,
                TotalCount = total,
                CurrentPage = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<DishDetailsDto> GetDishDetailsAsync(Guid id)
        {
            var dish = await _context.Dishes
                .Include(d => d.Ratings)
                .Include(d => d.DishCategories)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null)
                throw new KeyNotFoundException($"Dish with id {id} not found.");

            return new DishDetailsDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                Description = dish.Description,
                IsVegetarian = dish.IsVegetarian,
                Photo = dish.Photo,
                Categories = dish.DishCategories.Select(dc => dc.Category).ToList(),
                AverageRating = dish.Ratings.Any() ? dish.Ratings.Average(r => r.Value) : 0
            };
        }
    }
}
