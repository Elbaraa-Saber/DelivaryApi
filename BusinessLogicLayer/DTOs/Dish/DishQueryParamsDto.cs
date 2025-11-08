using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Common;

namespace BusinessLogicLayer.DTOs.Dish
{
    public class DishQueryParamsDto
    {
        public List<Category>? Categories { get; set; }

        [DefaultValue(false)]
        public bool Vegetarian { get; set; } = false;

        [DefaultValue(DishSorting.NameAsc)]
        public DishSorting? Sorting { get; set; } = DishSorting.NameAsc;

        [DefaultValue(1)]
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
    }
}
