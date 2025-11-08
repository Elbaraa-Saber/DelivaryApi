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

            if (query.Categories is { Count: > 0 })
            {
                dishes = dishes.Where(d => d.DishCategories.Any(dc => query.Categories.Contains(dc.Category)));
            }

            if (query.Vegetarian)
            {
                dishes = dishes.Where(d => d.IsVegetarian);
            }

            dishes = (query.Sorting ?? DishSorting.NameAsc) switch
            {
                DishSorting.NameAsc => dishes.OrderBy(d => d.Name),
                DishSorting.NameDesc => dishes.OrderByDescending(d => d.Name),
                DishSorting.PriceAsc => dishes.OrderBy(d => d.Price),
                DishSorting.PriceDesc => dishes.OrderByDescending(d => d.Price),
                DishSorting.RatingAsc => dishes.OrderBy(d => d.Ratings.Any() ? d.Ratings.Average(r => r.Value) : 0),
                DishSorting.RatingDesc => dishes.OrderByDescending(d => d.Ratings.Any() ? d.Ratings.Average(r => r.Value) : 0),
                _ => dishes.OrderBy(d => d.Name)
            };

            var total = await dishes.CountAsync();

            var items = await dishes
                .Skip((query.Page - 1) * 10)
                .Take(10)
                .Select(d => new DishListItemDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Photo = d.Photo,
                    Rating = d.Ratings.Any() ? d.Ratings.Average(r => r.Value) : 0,
                    Categories = d.DishCategories.Select(dc => dc.Category).ToList()
                })
                .ToListAsync();

            return new DishListResponseDto
            {
                Dishes = items,
                Pagination = new PaginationDto
                {
                    Current = query.Page,
                    Size = 10,
                    Count = total
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
                Image = dish.Photo,                 
                Vegetarian = dish.IsVegetarian,
                Rating = dish.Ratings.Any() ? dish.Ratings.Average(r => r.Value) : 0,
                Category = dish.DishCategories
                    .Select(dc => dc.Category)
                    .FirstOrDefault()               
            };
        }
    }
}
