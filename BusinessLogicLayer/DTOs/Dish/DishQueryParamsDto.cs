using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs.Dish
{
    public class DishQueryParamsDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<Category>? Categories { get; set; }
        public string? Search { get; set; }
        public bool? IsVegetarian { get; set; }
    }
}
