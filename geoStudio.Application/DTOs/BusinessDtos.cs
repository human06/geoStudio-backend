using geoStudio.Domain.Enums;

namespace geoStudio.Application.DTOs;

/// <summary>Request payload for creating a new business profile.</summary>
public record CreateBusinessRequest(
    string Name,
    string? WebsiteUrl = null,
    string? IndustryCategory = null,
    string? Description = null,
    string? LogoUrl = null);

/// <summary>Request payload for updating an existing business profile.</summary>
public record UpdateBusinessRequest(
    string? Name = null,
    string? WebsiteUrl = null,
    string? IndustryCategory = null,
    string? Description = null,
    string? LogoUrl = null);

/// <summary>Full business profile response.</summary>
public record BusinessResponse(
    Guid Id,
    Guid OwnerUserId,
    string Name,
    string? WebsiteUrl,
    string? IndustryCategory,
    string? Description,
    string? LogoUrl,
    SubscriptionTier SubscriptionTier,
    DateTime CreatedAt,
    DateTime UpdatedAt);

/// <summary>Paginated list wrapper.</summary>
public record PagedResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
