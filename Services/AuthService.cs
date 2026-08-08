using CloudinaryDotNet;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Auth;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Services
{
    public class AuthService : ServiceBaseFunctions, IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthService(
            AppDbContext context,
            ITokenService tokenService,
            IEmailService emailService,
            IConfiguration config)

        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
            _config = config;

        }
        public async Task<ApiResponse<UserAuthDto>> RegisterAsync(RegisterDto dto, string? avator)
        {
            try
            {
                var result = await CreateUserAccountAsync(dto, RoleNames.DefaultSignupRole, UserStatus.Pending,
                    mustChangePassword: false, avator);

                if (!result.Success)
                {
                    return Fail<UserAuthDto>(result.Message);
                }
                return Ok(result.Data!, "Registration successful! Your account is pending admin approval — you'll be able to log in once approved.");
            }
            catch (Exception ex)
            {
                return Fail<UserAuthDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<UserAuthDto>> RegisterWithRoleAsync(RegisterDto dto, string roleName, bool mustChangePassword, string? avatarUrl = null)
        {
            try
            {
                var result = await CreateUserAccountAsync(dto, roleName, UserStatus.Active, mustChangePassword, avatarUrl);
                if (!result.Success) return result;
                return Ok(result.Data, mustChangePassword
                    ? "Account created successfully. The user must change their password on first login."
                    : "Account created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserAuthDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u =>
                        u.UserName.ToLower() == dto.UserNameOrEmail.ToLower() ||
                        u.Email.ToLower() == dto.UserNameOrEmail.ToLower());

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                    return Fail<AuthResponseDto>("Invalid username/email or password.");

                switch (user.Status)
                {
                    case UserStatus.Pending:
                        return Fail<AuthResponseDto>("Your account is still pending admin approval.");
                    case UserStatus.Rejected:
                        return Fail<AuthResponseDto>("Your account request was rejected. Please contact an admin.");
                    case UserStatus.Inactive:
                        return Fail<AuthResponseDto>("Your account has been deactivated. Please contact an admin.");
                }

                var authResponse = await IssueTokensAsync(user);
                return Ok(authResponse, "Login successful.");
            }
            catch (Exception ex)
            {
                return Fail<AuthResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }

        }
        public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var existing = await _context.RefreshTokens
                                            .Include(refToken => refToken.User)
                                            .ThenInclude(u => u.Role)
                                            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

                if (existing == null || !existing.IsActive)
                    return Fail<AuthResponseDto>("Invalid or expired refresh token, please login again");
                if (existing.User.Status != UserStatus.Active)
                    return Fail<AuthResponseDto>("Your account is no longer active. Please contact an admin.");
                // rotate: revoke the old refresh token and issue a fresh pair
                existing.RevokedAt = DateTime.UtcNow;

                var authResponse = await IssueTokensAsync(existing.User);
                existing.ReplacedByToken = authResponse.RefreshToken;
                await _context.SaveChangesAsync();
                return Ok(authResponse, "Token refreshed successfully.");
            }
            catch (Exception ex)
            {
                return Fail<AuthResponseDto>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return Fail<bool>("User not found.");

                if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.Password))
                    return Fail<bool>("Current password is incorrect.");

                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                await _context.SaveChangesAsync();

                return Ok(true, "Password changed successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }

        }
        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            try
            {
                var resetToken = await _context.PasswordResetTokens
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Token == dto.Token);

                if (resetToken == null || !resetToken.IsActive)
                    return Fail<bool>("This reset link is invalid or has expired. Please request a new one.");

                resetToken.User.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                resetToken.IsUsed = true;

                // revoke all active sessions for safety since the password changed
                var activeRefreshTokens = await _context.RefreshTokens
                    .Where(rt => rt.UserId == resetToken.UserId && rt.RevokedAt == null)
                    .ToListAsync();
                foreach (var rt in activeRefreshTokens) rt.RevokedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(true, "Password reset successfully. You can now log in with your new password.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }

        }
        public async Task<ApiResponse<bool>> ForgotPasswordAsync(string email)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                const string genericMessage = "If an account with that email exists, a password reset link has been sent.";
                if (existingUser == null)
                    return Ok(true, genericMessage);
                var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
                var resetToken = new PasswordResetToken
                {
                    UserId = existingUser.UserId,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(1),
                };
                await _context.PasswordResetTokens.AddAsync(resetToken);
                await _context.SaveChangesAsync();

                var frontendBaseUrl = _config["FrontendSettings:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:3000";
                var resetLink = $"{frontendBaseUrl}/reset-password?token={token}";
                await _emailService.SendPasswordResetEmailAsync(existingUser.Email, existingUser.FullName, resetLink);
                return Ok(true, genericMessage);
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }

        }
        public async Task<ApiResponse<bool>> LogoutAsync(string refreshToken)
        {
            try
            {
                var existing = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
                if (existing != null && existing.IsActive)
                {
                    existing.RevokedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                return Ok(true, "Logged out successfully.");
            }
            catch (Exception ex)
            {
                return Fail<bool>(ex.InnerException?.Message ?? ex.Message);
            }

        }




        //---------------------------------HELPER----------------------------------------
        private async Task<AuthResponseDto> IssueTokensAsync(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshTokenValue = _tokenService.GenerateRefreshTokenValue();
            var refreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(_tokenService.RefreshTokenExpiryDays())
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                AccessTokenExpiresAt = _tokenService.GetAccessTokenExpiry(),
                User = new UserAuthDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role?.RoleName ?? string.Empty,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                }
            };

        }

        private async Task<ApiResponse<UserAuthDto>> CreateUserAccountAsync(
     RegisterDto dto, string roleName, UserStatus status, bool mustChangePassword, string? avatarUrl)
        {
            var userNameTaken = await _context.Users.AnyAsync(u => u.UserName.ToLower() == dto.Username.ToLower());
            if (userNameTaken)
                return Fail<UserAuthDto>("This username is already taken.");

            var emailTaken = await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower());
            if (emailTaken)
                return Fail<UserAuthDto>("An account with this email already exists.");

            var role = await _context.UserRole.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
                return Fail<UserAuthDto>($"Role '{roleName}' is not configured. Please contact an admin.");

            var user = new User
            {
                UserName = dto.Username,
                FullName = dto.Fullname,
                Email = dto.Email,
                ProfilePictureUrl = avatarUrl ?? "",
                PhoneNumber = dto.PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Status = status,
                RoleId = role.RoleId,
                MustChangePassword = mustChangePassword,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var result = new UserAuthDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl,
                RoleId = role.RoleId,
                RoleName = role.RoleName,
            };

            return Ok(result, "Account created successfully.");
        }
    }
}
