namespace geoStudio.Domain.Entities;

/// <summary>Represents a platform user account.</summary>
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<BusinessProfile> OwnedBusinesses { get; set; } = [];
    public ICollection<TeamMember> TeamMemberships { get; set; } = [];
    public ICollection<TeamMember> InvitationsSent { get; set; } = [];
    public ICollection<Audit> CreatedAudits { get; set; } = [];
    public ICollection<Report> CreatedReports { get; set; } = [];
    public ICollection<ReportSchedule> CreatedSchedules { get; set; } = [];
    public ICollection<geoStudio.Domain.Entities.Task> AssignedTasks { get; set; } = [];
    public ICollection<geoStudio.Domain.Entities.Task> CreatedTasks { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
    public ICollection<NotificationPreference> NotificationPreferences { get; set; } = [];
}
