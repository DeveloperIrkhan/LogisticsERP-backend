using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.DTOs.User
{
    public class UserResponseDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserStatus Status { get; set; } = UserStatus.INACTIVE;
        public string? ProfilePictureUrl { get; set; } = string.Empty;

        //relationship with Role
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
