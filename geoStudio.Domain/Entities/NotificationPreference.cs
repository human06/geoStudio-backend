namespace geoStudio.Domain.Entities;

/// <summary>Represents a user's notification preferences for a specific business.</summary>
public class NotificationPreference
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid BusinessId { get; set; }

    // Audit Completed Notifications
    public bool AuditCompletedEmail { get; set; } = true;
    public bool AuditCompletedInApp { get; set; } = true;

    // Gap Found Notifications
    public bool GapFoundEmail { get; set; } = true;
    public bool GapFoundInApp { get; set; } = true;
    public bool GapFoundSlack { get; set; } = false;

    // Task Assigned Notifications
    public bool TaskAssignedEmail { get; set; } = true;
    public bool TaskAssignedInApp { get; set; } = true;

    // Report Ready Notifications
    public bool ReportReadyEmail { get; set; } = true;
    public bool ReportReadyInApp { get; set; } = true;

    // Quiet Hours
    public TimeSpan? QuietHoursStart { get; set; }
    public TimeSpan? QuietHoursEnd { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public BusinessProfile Business { get; set; } = null!;
}
