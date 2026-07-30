using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Roles;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class RoleService : ServiceBaseFunctions, IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<RoleResponseDto>>> GetAllAsync()
        {
            try
            {
                var roles = await _context.UserRole.Include(r => r.Users).ToListAsync();
                var result = roles.Select(r => new RoleResponseDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    UserCount = r.Users.Count,
                }).OrderBy(r => r.RoleName).ToList();

                return Ok(result, $"{result.Count} role(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<RoleResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<RoleResponseDto>> CreateAsync(RoleCreateDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.RoleName))
                    return Fail<RoleResponseDto>("Role name is required.");

                var exists = await _context.UserRole.AnyAsync(r => r.RoleName.ToLower() == dto.RoleName.ToLower());
                if (exists) return Fail<RoleResponseDto>("A role with this name already exists.");

                var role = new Role { RoleName = dto.RoleName };
                await _context.UserRole.AddAsync(role);
                await _context.SaveChangesAsync();

                return Ok(new RoleResponseDto { RoleId = role.RoleId, RoleName = role.RoleName, UserCount = 0 }, "Role created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<RoleResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }

}
