using BusinessLogicLayer.DTOs.User;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> RegisterAsync(UserRegisterDto dto);
        Task<TokenResponseDto> LoginAsync(UserLoginDto dto);
        Task<UserProfileDto> GetProfileAsync(Guid userId);
        Task EditProfileAsync(Guid userId, UserProfileEditDto dto);
    }
}
