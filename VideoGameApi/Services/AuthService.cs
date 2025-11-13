using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using VideoGameApi.Data;
using VideoGameApi.Entities;
using VideoGameApi.Helpers;
using VideoGameApi.Model;
using VideoGameApi.Services.Interfaces;

namespace VideoGameApi.Services
{
    public class AuthService(VideoGameDBContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserRegDto request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }
            var user = new User();

            var hashedPassword = new PasswordHasher<Entities.User>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.Password = hashedPassword;
            user.Email = request.Email;
            user.Phone = request.Phone;
            user.Name = request.Name;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;

        }
        public async Task<TokenResponseDto?> Login(UserLoginDto request)
        {

            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null)
            {
                return null;
            }

            var isPasswordCorrect = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.Password, request.Password)
                == PasswordVerificationResult.Failed;

            if (isPasswordCorrect)
            {
                return null;
            }

            return await CreateRefreshTokens(user);
        }

        public async Task<ProfileDto?> GetLoginUser(string userId)
        {

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user is null)
            {
                return null;
            }

            return new ProfileDto
            {
                Name = user.Name ?? string.Empty,
                Username = user.Username ?? string.Empty,
                Role = user.Role ?? string.Empty,
                UserId = user?.Id.ToString(),
            };
        }

        public async Task<string?> UpdateUserProfile(ProfileDto payload)
        {
            var guidId = Guid.Parse(payload.UserId);
            var user = await context.Users.FindAsync(guidId);
            if (user is null)
            {
                return null;
            }
            user.Name = payload.Name;
            user.Phone = payload.Phone;
            user.Role = payload.Role;

            await context.SaveChangesAsync();

            return "Profile updated successfully";
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var ring = RandomNumberGenerator.Create();
            ring.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refershToken = GenerateRefreshToken();
            user.RefreshToken = refershToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return refershToken;
        }

        private async Task<User?> ValidateRefresTokenAsync(string userId, string refershToken)
        {
            var user = await context.Users.FindAsync(Guid.Parse(userId));
            if (user is null || user.RefreshToken != refershToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        public async Task<TokenResponseDto> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefresTokenAsync(request.UserId, request.RefreshToken);
            if (user == null) { return null; }

            return await CreateRefreshTokens(user);


        }

        private async Task<TokenResponseDto> CreateRefreshTokens(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = new AuthHelpers(configuration).CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }
    }

}
