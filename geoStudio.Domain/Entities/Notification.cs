namespace geoStudio.Domain.Entities;

/// <summary>Represents an in-app or email notification sent to a user.</summary>
public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid BusinessId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string NotificationType { get; set; } = string.Empty; // "audit_completed", "gap_found", "task_assigned", "report_ready", etc.

    public string? RelatedEntityType { get; set; } // "audit", "task", "report", etc.
    public Guid? RelatedEntityId { get; set; }

    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User User { get; set; } = null!;
    public BusinessProfile Business { get; set; } = null!;
}
