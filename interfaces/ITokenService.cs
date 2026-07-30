using LogisticsERP.API.Models;
using System.Security.Claims;

namespace LogisticsERP.API.interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        DateTime GetAccessTokenExpiry();
        string GenerateRefreshTokenValue();
        int RefreshTokenExpiryDays();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
