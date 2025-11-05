namespace BusinessLogicLayer.DTOs.Dish
{
    public class DishListResponseDto
    {
        public List<DishListItemDto> Dishes { get; set; } = new();
        public PaginationDto Pagination { get; set; } = new();
    }

    public class PaginationDto
    {
        public int Size { get; set; }
        public int Count { get; set; }
        public int Current { get; set; }
    }
}