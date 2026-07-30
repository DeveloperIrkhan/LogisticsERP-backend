using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class RefreshToken
    {
        [Key]
        public string RefreshTokenId { get; set; } = $"PRCS-RTK-{Guid.NewGuid()}";
        [Required]
        public string Token { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } 

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }

        [NotMapped]
        public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;

    }
}
