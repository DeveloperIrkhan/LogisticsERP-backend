using LogisticsERP.API.DTOs.Roles;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<List<RoleResponseDto>>> GetAllAsync();
        Task<ApiResponse<RoleResponseDto>> CreateAsync(RoleCreateDto dto);

    }
}
