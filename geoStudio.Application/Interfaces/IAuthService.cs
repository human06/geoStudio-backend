using geoStudio.Application.DTOs;

namespace geoStudio.Application.Interfaces;

/// <summary>Authentication service contract.</summary>
public interface IAuthService
{
    /// <summary>Registers a new user and returns auth tokens.</summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>Authenticates a user and returns auth tokens.</summary>
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    /// <summary>Rotates the access/refresh token pair.</summary>
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);

    /// <summary>Invalidates the current refresh token (logout).</summary>
    Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
}
