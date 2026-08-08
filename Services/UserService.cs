using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.User;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class UserService : ServiceBaseFunctions, IUserService

    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public UserService(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ApiResponse<List<UserResponseDto>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.Include(u => u.Role)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
                return Ok(MapList(users), $"{users.Count} user(s) found.");
            }
            catch (Exception ex)
            {
                return Fail<List<UserResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<UserResponseDto>> GetByIdAsync(string id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null) return Fail<UserResponseDto>("User not found.");
                return Ok(MapOne(user), "User fetched successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<UserResponseDto>>> GetPendingApprovalsAsync()
        {
            try
            {
                var users = await _context.Users.Include(u => u.Role)
                    .Where(u => u.Status == UserStatus.Pending)
                    .OrderBy(u => u.CreatedAt)
                    .ToListAsync();
                return Ok(MapList(users), $"{users.Count} account(s) awaiting approval.");
            }
            catch (Exception ex)
            {
                return Fail<List<UserResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<List<UserResponseDto>>> GetByStatusAsync(UserStatus status)
        {
            try
            {
                var users = await _context.Users.Include(u => u.Role)
                    .Where(u => u.Status == status)
                    .OrderByDescending(u => u.CreatedAt)
                    .ToListAsync();
                return Ok(MapList(users), $"{users.Count} user(s) with status {status}.");
            }
            catch (Exception ex)
            {
                return Fail<List<UserResponseDto>>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<UserResponseDto>> ApproveAsync(string id, ApproveUserDto dto)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null) return Fail<UserResponseDto>("User not found.");

                if (!string.IsNullOrWhiteSpace(dto.RoleId))
                {
                    var role = await _context.UserRole.FindAsync(dto.RoleId);
                    if (role == null) return Fail<UserResponseDto>("Selected role was not found.");
                    user.RoleId = dto.RoleId;
                    user.Role = role;
                }

                user.Status = UserStatus.Active;
                user.ApprovedBy = dto.ApprovedBy;
                user.ApprovedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                //await _emailService.SendAccountApprovedEmailAsync(user.Email, user.FullName);

                return Ok(MapOne(user), "User approved successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<UserResponseDto>> RejectAsync(string id, RejectUserDto dto)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null) return Fail<UserResponseDto>("User not found.");

                user.Status = UserStatus.Rejected;
                user.ApprovedBy = dto.ApprovedBy;
                user.ApprovedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                //await _emailService.SendAccountRejectedEmailAsync(user.Email, user.FullName, dto.Reason);

                return Ok(MapOne(user), "User rejected.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<UserResponseDto>> DeactivateAsync(string id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null) return Fail<UserResponseDto>("User not found.");

                user.Status = UserStatus.Inactive;

                var activeRefreshTokens = await _context.RefreshTokens
                    .Where(rt => rt.UserId == id && rt.RevokedAt == null)
                    .ToListAsync();
                foreach (var rt in activeRefreshTokens) rt.RevokedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(MapOne(user), "User deactivated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<UserResponseDto>> ReactivateAsync(string id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null) return Fail<UserResponseDto>("User not found.");

                user.Status = UserStatus.Active;
                await _context.SaveChangesAsync();
                return Ok(MapOne(user), "User reactivated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<UserResponseDto>> UpdateRoleAsync(string id, UpdateUserRoleDto dto)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null) return Fail<UserResponseDto>("User not found.");

                var role = await _context.UserRole.FindAsync(dto.RoleId);
                if (role == null) return Fail<UserResponseDto>("Selected role was not found.");

                user.RoleId = dto.RoleId;
                user.Role = role;

                await _context.SaveChangesAsync();
                return Ok(MapOne(user), "User role updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<UserResponseDto>> UpdateProfileAsync(string id, UpdateUserProfileDto dto, string? pictureUrl)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null) return Fail<UserResponseDto>("User not found.");

                if (dto.FullName != null) user.FullName = dto.FullName;
                if (dto.PhoneNumber != null) user.PhoneNumber = dto.PhoneNumber;
                if (dto.Avatar!= null) user.ProfilePictureUrl = pictureUrl;

                await _context.SaveChangesAsync();
                return Ok(MapOne(user), "Profile updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return Fail<bool>("User not found.");

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(true, "User deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ── Helpers ─────────────────────────────────────────────
        private static UserResponseDto MapOne(User user) => new()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Status = user.Status,
            ProfilePictureUrl = user.ProfilePictureUrl,
            RoleId = user.RoleId,
            RoleName = user.Role?.RoleName ?? string.Empty,
            CreatedAt = user.CreatedAt,
            ApprovedBy = user.ApprovedBy,
            ApprovedAt = user.ApprovedAt,
        };

        private static List<UserResponseDto> MapList(List<User> users) => users.Select(MapOne).ToList();
    }

}
