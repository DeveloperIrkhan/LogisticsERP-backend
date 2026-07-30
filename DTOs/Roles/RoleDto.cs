namespace LogisticsERP.API.DTOs.Roles
{
    public class RoleCreateDto
    {
        public string RoleName { get; set; } = string.Empty;
    }

    public class RoleResponseDto
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }
}
