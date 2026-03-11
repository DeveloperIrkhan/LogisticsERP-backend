namespace LogisticsERP.API.Models
{
    public class Role
    {
        public string RoleId { get; set; } = $"PRCS-ROL-{Guid.NewGuid()}";
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; } = [];
    }
}
