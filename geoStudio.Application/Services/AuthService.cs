using geoStudio.Application.DTOs;
using geoStudio.Application.Interfaces;
using geoStudio.Domain.Entities;
using Task = System.Threading.Tasks.Task;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace geoStudio.Application.Services;

/// <inheritdoc cref="IAuthService"/>
public sealed class AuthService(
    IUserRepository userRepository,
    IJwtTokenProvider jwtTokenProvider,
    IConfiguration configuration,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly int _refreshTokenExpiryDays =
        int.Parse(configuration["Jwt:RefreshTokenExpiryDays"] ?? "7");

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var emailNormalized = request.Email.Trim().ToLower();

        if (await userRepository.EmailExistsAsync(emailNormalized, cancellationToken))
            throw new InvalidOperationException($"Email '{emailNormalized}' is already registered.");

        var user = new User
        {
            Email = emailNormalized,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Phone = request.Phone?.Trim()
        };

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User {UserId} registered with email {Email}", user.Id, user.Email);

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var emailNormalized = request.Email.Trim().ToLower();
        var user = await userRepository.GetByEmailAsync(emailNormalized, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var authResponse = BuildAuthResponse(user);

        user.RefreshToken = authResponse.RefreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        userRepository.Update(user);
        await userRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User {UserId} logged in", user.Id);

        return authResponse;
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var principal = jwtTokenProvider.GetPrincipalFromExpiredToken(request.AccessToken)
            ?? throw new UnauthorizedAccessException("Invalid access token.");

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                          ?? principal.FindFirst("sub")
            ?? throw new UnauthorizedAccessException("Invalid token claims.");

        var userId = Guid.Parse(userIdClaim.Value);
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        if (user.RefreshToken != request.RefreshToken ||
            user.RefreshTokenExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token is invalid or expired.");

        var authResponse = BuildAuthResponse(user);

        user.RefreshToken = authResponse.RefreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        userRepository.Update(user);
        await userRepository.SaveChangesAsync(cancellationToken);

        return authResponse;
    }

    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null) return;

        user.RefreshToken = null;
        user.RefreshTokenExpiresAt = null;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User {UserId} logged out", userId);
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        var accessToken = jwtTokenProvider.GenerateAccessToken(user);
        var refreshToken = jwtTokenProvider.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        return new AuthResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: expiresAt,
            User: new UserSummary(
                Id: user.Id,
                Email: user.Email,
                FirstName: user.FirstName,
                LastName: user.LastName,
                AvatarUrl: user.AvatarUrl));
    }
}
