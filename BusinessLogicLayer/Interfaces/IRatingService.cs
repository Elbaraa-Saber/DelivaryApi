using BusinessLogicLayer.DTOs.Rating;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRatingService
    {
        Task<bool> CanUserRateDishAsync(Guid userId, Guid dishId);
        Task AddOrUpdateRatingAsync(Guid userId, Guid dishId, RatingDto ratingDto);
    }
}
