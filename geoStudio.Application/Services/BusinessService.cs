using geoStudio.Application.DTOs;
using geoStudio.Application.Interfaces;
using geoStudio.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace geoStudio.Application.Services;

/// <inheritdoc cref="IBusinessService"/>
public sealed class BusinessService(
    IRepository<BusinessProfile> businessRepository,
    ILogger<BusinessService> logger) : IBusinessService
{
    public async Task<BusinessResponse> CreateAsync(
        Guid ownerUserId,
        CreateBusinessRequest request,
        CancellationToken cancellationToken = default)
    {
        var business = new BusinessProfile
        {
            OwnerUserId = ownerUserId,
            Name = request.Name.Trim(),
            WebsiteUrl = request.WebsiteUrl?.Trim(),
            IndustryCategory = request.IndustryCategory?.Trim(),
            Description = request.Description?.Trim(),
            LogoUrl = request.LogoUrl?.Trim()
        };

        await businessRepository.AddAsync(business, cancellationToken);
        await businessRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Business {BusinessId} created for user {UserId}", business.Id, ownerUserId);

        return MapToResponse(business);
    }

    public async Task<BusinessResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var business = await businessRepository.GetByIdAsync(id, cancellationToken);
        return business is null ? null : MapToResponse(business);
    }

    public async Task<IEnumerable<BusinessResponse>> GetByOwnerAsync(
        Guid ownerUserId,
        CancellationToken cancellationToken = default)
    {
        var businesses = await businessRepository.FindAsync(b => b.OwnerUserId == ownerUserId, cancellationToken);
        return businesses.Select(MapToResponse);
    }

    public async Task<BusinessResponse> UpdateAsync(
        Guid id,
        UpdateBusinessRequest request,
        CancellationToken cancellationToken = default)
    {
        var business = await businessRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Business '{id}' not found.");

        if (request.Name is not null) business.Name = request.Name.Trim();
        if (request.WebsiteUrl is not null) business.WebsiteUrl = request.WebsiteUrl.Trim();
        if (request.IndustryCategory is not null) business.IndustryCategory = request.IndustryCategory.Trim();
        if (request.Description is not null) business.Description = request.Description.Trim();
        if (request.LogoUrl is not null) business.LogoUrl = request.LogoUrl.Trim();

        businessRepository.Update(business);
        await businessRepository.SaveChangesAsync(cancellationToken);

        return MapToResponse(business);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var business = await businessRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Business '{id}' not found.");

        businessRepository.Remove(business);
        await businessRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Business {BusinessId} deleted", id);
    }

    private static BusinessResponse MapToResponse(BusinessProfile b) => new(
        Id: b.Id,
        OwnerUserId: b.OwnerUserId,
        Name: b.Name,
        WebsiteUrl: b.WebsiteUrl,
        IndustryCategory: b.IndustryCategory,
        Description: b.Description,
        LogoUrl: b.LogoUrl,
        SubscriptionTier: b.SubscriptionTier,
        CreatedAt: b.CreatedAt,
        UpdatedAt: b.UpdatedAt);
}
