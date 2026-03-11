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
        public UserStatus Status { get; set; } = UserStatus.INACTIVE;
        public string? ProfilePictureUrl { get; set; } = string.Empty;

        //relationship with Role
        [ForeignKey("RoleId")]
        public string RoleId { get; set; }
        public Role Role { get; set; } 
    }
}
