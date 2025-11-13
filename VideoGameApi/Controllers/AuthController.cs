using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VideoGameApi.Entities;
using VideoGameApi.Model;
using VideoGameApi.Services.Interfaces;

namespace VideoGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegDto request)
        {

            if ((string.IsNullOrWhiteSpace(request.Username?.Trim()) && string.IsNullOrWhiteSpace(request.Email?.Trim()) || string.IsNullOrWhiteSpace(request.Password)))
            {
                return BadRequest("Username/email and password are required");
            }

            var user = await authService.RegisterAsync(request);
            if (user is null)
            {
                return BadRequest("User already exist.");
            }

            return Ok(user);


        }


        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
        {

            var res = await authService.Login(request);
            if (res is null)
            {
                return BadRequest("Invalid login credentials.");
            }
            return Ok(res);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("me")]
        public async Task<ActionResult<ProfileDto>> GetLoginUser()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User information not found in token.");
            }

            // Get full profile using your service
            var user = await authService.GetLoginUser(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }


        [Authorize]
        [HttpPatch("me")]
        public async Task<ActionResult<string>> UpdateUserProfile(ProfileDto payload)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User authenticated.");
            }

            var user = await authService.UpdateUserProfile(payload);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request)
        {
            var tokens = await authService.RefreshTokensAsync(request);
            if(tokens == null)
            {
                return BadRequest("Invalid refresh token");
            }
            return Ok(tokens);
        }
    }
}
