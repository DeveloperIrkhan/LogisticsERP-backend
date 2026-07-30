using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LogisticsERP.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration configuration)
        {
            _config = configuration;
        }

        private string Secret => _config["JwtSettings:Secret"]
            ?? throw new InvalidOperationException("JwtSettings:Secret is not configured.");
        private string Issuer => _config["JwtSettings:Issuer"] ?? "LogisticsERP.API";
        private string Audience => _config["JwtSettings:Audience"] ?? "LogisticsERP.Client";
        private int AccessTokenMinutes => int.TryParse(_config["JwtSettings:AccessTokenMinutes"], out var m) ? m : 30;
        private int RefreshTokenDays => int.TryParse(_config["JwtSettings:RefreshTokenDays"], out var d) ? d : 7;
        public DateTime GetAccessTokenExpiry() => DateTime.UtcNow.AddMinutes(AccessTokenMinutes);
        public int RefreshTokenExpiryDays() => RefreshTokenDays;
        public string GenerateAccessToken(User user)
        {
            var Claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserId),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role?.RoleName ?? string.Empty),
                new("fullName", user.FullName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: Claims,
                expires: GetAccessTokenExpiry(),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshTokenValue()
        {
            var rendom = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(rendom);
        }


        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
                ValidateLifetime = false, // we WANT to read an expired token here
            };

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var principal = handler.ValidateToken(token, validationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}
