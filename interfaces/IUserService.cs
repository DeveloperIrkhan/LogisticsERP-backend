using LogisticsERP.API.DTOs.User;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserResponseDto>>> GetAllAsync();
        Task<ApiResponse<UserResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<UserResponseDto>>> GetPendingApprovalsAsync();
        Task<ApiResponse<List<UserResponseDto>>> GetByStatusAsync(UserStatus status);

        Task<ApiResponse<UserResponseDto>> ApproveAsync(string id, ApproveUserDto dto);
        Task<ApiResponse<UserResponseDto>> RejectAsync(string id, RejectUserDto dto);
        Task<ApiResponse<UserResponseDto>> DeactivateAsync(string id);
        Task<ApiResponse<UserResponseDto>> ReactivateAsync(string id);
        Task<ApiResponse<UserResponseDto>> UpdateRoleAsync(string id, UpdateUserRoleDto dto);
        Task<ApiResponse<UserResponseDto>> UpdateProfileAsync(string id, UpdateUserProfileDto dto, string? pictureUrl);
        Task<ApiResponse<bool>> DeleteAsync(string id);

    }
}
