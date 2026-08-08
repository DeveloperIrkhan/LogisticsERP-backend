using LogisticsERP.API.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class User
    {
        [Key]public string UserId { get; set; } = $"PRCS-USR-{Guid.NewGuid()}";
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserStatus Status { get; set; } = UserStatus.Pending;
        public string? ProfilePictureUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool MustChangePassword { get; set; } = false;

        //relationship with Role
        [ForeignKey("RoleId")]
        public string RoleId { get; set; } = string.Empty;
        public Role Role { get; set; } = new();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = [];
    }
}
