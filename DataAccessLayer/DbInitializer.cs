using DataAccessLayer.Common;
using DataAccessLayer.Context;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Dishes.Any())
                return;

            var dishes = new List<Dish>
            {
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Pizza Margherita",
                    Price = 500,
                    Description = "Classic Italian pizza with tomato sauce and cheese",
                    IsVegetarian = true,
                    Photo = "pizza.jpg",
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = DateTime.UtcNow
                },
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Chicken Wok",
                    Price = 450,
                    Description = "Asian noodles with chicken and vegetables",
                    IsVegetarian = false,
                    Photo = "wok.jpg",
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = DateTime.UtcNow
                },
                new Dish
                {
                    Id = Guid.NewGuid(),
                    Name = "Chocolate Cake",
                    Price = 300,
                    Description = "Delicious dessert with chocolate cream",
                    IsVegetarian = true,
                    Photo = "cake.jpg",
                    CreateDateTime = DateTime.UtcNow,
                    ModifyDateTime = DateTime.UtcNow
                }
            };

            context.Dishes.AddRange(dishes);
            context.SaveChanges();

            // (DishCategory)
            var categories = new List<DishCategory>
            {
                new DishCategory { DishId = dishes[0].Id, Category = Category.Pizza },
                new DishCategory { DishId = dishes[1].Id, Category = Category.Wok },
                new DishCategory { DishId = dishes[2].Id, Category = Category.Desert }
            };

            context.DishCategories.AddRange(categories);
            context.SaveChanges();
        }
    }
}
