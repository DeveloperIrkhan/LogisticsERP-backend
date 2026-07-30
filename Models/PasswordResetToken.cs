using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class PasswordResetToken
    {
        [Key]
        public string PasswordResetTokenId { get; set; } = $"PRCS-PRT-{Guid.NewGuid()}";
        [Required]
        public string Token { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = new();

        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public bool IsActive => !IsUsed && DateTime.UtcNow < ExpiresAt;

    }
}
