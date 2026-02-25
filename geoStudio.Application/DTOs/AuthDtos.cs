namespace geoStudio.Application.DTOs;

/// <summary>Request payload for user registration.</summary>
public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Phone = null);

/// <summary>Request payload for user login.</summary>
public record LoginRequest(
    string Email,
    string Password);

/// <summary>Request payload for token refresh.</summary>
public record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken);

/// <summary>Authentication response containing tokens and basic user info.</summary>
public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserSummary User);

/// <summary>Basic user info embedded in auth responses.</summary>
public record UserSummary(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? AvatarUrl);
