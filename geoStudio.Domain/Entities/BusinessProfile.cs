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
    public int MaxLocations { get; set; } = 1;
    public int MaxAuditsPerMonth { get; set; } = 50;
    public int MaxTeamMembers { get; set; } = 3;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User Owner { get; set; } = null!;
    public ICollection<BusinessLocation> Locations { get; set; } = [];
    public ICollection<Competitor> Competitors { get; set; } = [];
    public ICollection<Persona> Personas { get; set; } = [];
    public ICollection<TeamMember> TeamMembers { get; set; } = [];
    public ICollection<Audit> Audits { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
    public ICollection<ReportSchedule> ReportSchedules { get; set; } = [];
    public ICollection<geoStudio.Domain.Entities.Task> Tasks { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
    public ICollection<NotificationPreference> NotificationPreferences { get; set; } = [];
    public ICollection<AlertRule> AlertRules { get; set; } = [];
}
