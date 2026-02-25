using geoStudio.Domain.Enums;

namespace geoStudio.Domain.Entities;

/// <summary>Represents a business registered on the geoStudio platform.</summary>
public class BusinessProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OwnerUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? WebsiteUrl { get; set; }
    public string? IndustryCategory { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public SubscriptionTier SubscriptionTier { get; set; } = SubscriptionTier.Free;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User Owner { get; set; } = null!;
    public ICollection<TeamMember> TeamMembers { get; set; } = [];
}
