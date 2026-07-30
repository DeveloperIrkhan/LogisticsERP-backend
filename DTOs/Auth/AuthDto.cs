using System.ComponentModel.DataAnnotations;

namespace LogisticsERP.API.DTOs.Auth
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress] 
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)] 
        public string Fullname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public IFormFile? Avator { get; set; } 
    }
    public class LoginDto
    {
        [Required] public string UserNameOrEmail { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; set; }
        public UserAuthDto User { get; set; } = new();
    }
    public class UserAuthDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
    }
    public class RefreshTokenRequestDto
    {
        [Required] public string RefreshToken { get; set; } = string.Empty;
    }

    public class ForgotPasswordDto
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required] public string Token { get; set; } = string.Empty;
        [Required, MinLength(6)] public string NewPassword { get; set; } = string.Empty;
    }

    public class ChangePasswordDto
    {
        [Required] public string CurrentPassword { get; set; } = string.Empty;
        [Required, MinLength(6)] public string NewPassword { get; set; } = string.Empty;
    }


}
