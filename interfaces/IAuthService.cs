using LogisticsERP.API.DTOs.Auth;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserAuthDto>> RegisterAsync(RegisterDto dto, string? avator);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
        Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse<bool>> LogoutAsync(string refreshToken);
        Task<ApiResponse<bool>> ForgotPasswordAsync(string email);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDto dto);
        Task<ApiResponse<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto);

    }
}
