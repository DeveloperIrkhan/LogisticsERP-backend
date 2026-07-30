using LogisticsERP.API.DTOs.Auth;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICloudinaryService _cloudinaryService;

        public AuthController(IAuthService authService, ICloudinaryService cloudinaryService)
        {
            _authService = authService; 
            _cloudinaryService = cloudinaryService;

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            string avator = "";
            if (dto == null) return BadRequest("Registration data is required.");
            if (dto.Avator != null)
            {
                var uploadImage = await _cloudinaryService.UploadImage(dto.Avator,
                    $"user-profile/{dto.Fullname}");
                avator = uploadImage.FileUrl;
            }
            var result = await _authService.RegisterAsync(dto, avator);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null) return BadRequest("Login data is required.");
            var result = await _authService.LoginAsync(dto);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
                return BadRequest("Refresh token is required.");
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
                return BadRequest("Refresh token is required.");
            var result = await _authService.LogoutAsync(dto.RefreshToken);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (dto == null) return BadRequest("Email is required.");
            var result = await _authService.ForgotPasswordAsync(dto.Email);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto == null) return BadRequest("Reset data is required.");
            var result = await _authService.ResetPasswordAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid session.");
            var result = await _authService.ChangePasswordAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Handy for the frontend to verify the access token / rehydrate the logged-in user on refresh.
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var user = new
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                UserName = User.FindFirstValue(ClaimTypes.Name),
                Email = User.FindFirstValue(ClaimTypes.Email),
                FullName = User.FindFirstValue("fullName"),
                Role = User.FindFirstValue(ClaimTypes.Role),
            };
            return Ok(new { success = true, message = "Session valid.", data = user });
        }
    }
}
