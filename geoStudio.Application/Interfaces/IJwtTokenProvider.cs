using System.Security.Claims;
using geoStudio.Domain.Entities;

namespace geoStudio.Application.Interfaces;

/// <summary>JWT token generation and validation contract.</summary>
public interface IJwtTokenProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
