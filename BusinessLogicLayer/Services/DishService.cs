using BusinessLogicLayer.DTOs;
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    // 🔸 أولاً: الواجهة
    public interface IDishService
    {
        Task<PagedResult<DishListItemDto>> GetDishesAsync(DishQueryParamsDto query);
        Task<DishDetailsDto> GetDishDetailsAsync(Guid id);
    }

    // 🔸 ثانياً: الكلاس (التنفيذ الفعلي للواجهة)
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;

        public DishService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DishListItemDto>> GetDishesAsync(DishQueryParamsDto query)
        {
            // هنا المنطق الخاص بجلب الأطباق
        }

        public async Task<DishDetailsDto> GetDishDetailsAsync(Guid id)
        {
            // هنا منطق جلب التفاصيل
        }
    }
}
