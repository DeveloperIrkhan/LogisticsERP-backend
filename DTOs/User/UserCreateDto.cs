using System.ComponentModel.DataAnnotations;

namespace LogisticsERP.API.DTOs.User
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters"), MinLength(3, ErrorMessage ="Username must contain al least 3 charecters!")] 
        public string UserName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Username is required")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "{0} is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? ProfilePictureUrl { get; set; }
        [Required(ErrorMessage = "{0} is required")] 
        public string RoleId { get; set; } = string.Empty;

    }
}
