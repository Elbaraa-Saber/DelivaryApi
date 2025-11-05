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

        public async Task<DishListResponseDto> GetDishesAsync(DishQueryParamsDto query)
        {
            var dishes = _context.Dishes
                .Include(d => d.DishCategories)
                .Include(d => d.Ratings)
                .AsQueryable();

            // تصفية حسب التصنيف
            if (query.Categories != null && query.Categories.Any())
            {
                dishes = dishes.Where(d =>
                    d.DishCategories.Any(dc => query.Categories.Contains(dc.Category)));
            }

            // تصفية نباتي
            if (query.IsVegetarian.HasValue)
                dishes = dishes.Where(d => d.IsVegetarian == query.IsVegetarian.Value);

            // الترتيب
            switch (query.Sorting)
            {
                case "NameAsc": dishes = dishes.OrderBy(d => d.Name); break;
                case "NameDesc": dishes = dishes.OrderByDescending(d => d.Name); break;
                case "PriceAsc": dishes = dishes.OrderBy(d => d.Price); break;
                case "PriceDesc": dishes = dishes.OrderByDescending(d => d.Price); break;
                case "RatingAsc": dishes = dishes.OrderBy(d => d.Ratings.Any() ? d.Ratings.Average(r => r.Value) : 0); break;
                case "RatingDesc": dishes = dishes.OrderByDescending(d => d.Ratings.Any() ? d.Ratings.Average(r => r.Value) : 0); break;
                default: dishes = dishes.OrderBy(d => d.Name); break;
            }

            var total = await dishes.CountAsync();
            var items = await dishes
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(d => new DishListItemDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    Photo = d.Photo,
                    IsVegetarian = d.IsVegetarian,
                    AverageRating = d.Ratings.Any() ? d.Ratings.Average(r => r.Value) : 0,
                    Categories = d.DishCategories.Select(dc => dc.Category).ToList()
                })
                .ToListAsync();

            return new DishListResponseDto
            {
                Dishes = items,
                Pagination = new PaginationDto
                {
                    Size = query.PageSize,
                    Count = total,
                    Current = query.Page
                }
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
                Description = dish.Description,
                Price = dish.Price,
                Photo = dish.Photo,
                IsVegetarian = dish.IsVegetarian,
                AverageRating = dish.Ratings.Any() ? dish.Ratings.Average(r => r.Value) : 0,
                Categories = dish.DishCategories.Select(dc => dc.Category).ToList()
            };
        }
    }
}
