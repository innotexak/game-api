using VideoGameApi.Entities;
using VideoGameApi.Model;

namespace VideoGameApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserRegDto request);

        Task<TokenResponseDto?> Login(UserLoginDto request);

        Task<ProfileDto?> GetLoginUser(string userId);

        Task<string?> UpdateUserProfile(ProfileDto payload);

        Task<TokenResponseDto> RefreshTokensAsync(RefreshTokenRequestDto request);

    }
}
