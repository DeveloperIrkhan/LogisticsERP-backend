namespace LogisticsERP.API.Models
{
    public class Role
    {
        public string RoleId { get; set; } = $"PRCS-ROL-{Guid.NewGuid()}";
        public string RoleName { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = [];
    }
}
