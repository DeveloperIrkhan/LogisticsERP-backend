using LogisticsERP.API.enums;

namespace LogisticsERP.API.DTOs.User
{
    public class UserResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }

    public class ApproveUserDto
    {
        // Admin confirms (or overrides) the role at approval time.
        public string RoleId { get; set; } = string.Empty;
        public string ApprovedBy { get; set; } = string.Empty;
    }

    public class RejectUserDto
    {
        public string ApprovedBy { get; set; } = string.Empty;
        public string? Reason { get; set; }
    }

    public class UpdateUserRoleDto
    {
        public string RoleId { get; set; } = string.Empty;
    }

    public class UpdateUserProfileDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Avatar { get; set; }

    }

}
