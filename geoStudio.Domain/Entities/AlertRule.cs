namespace geoStudio.Domain.Entities;

/// <summary>Represents an automated alert rule that triggers notifications based on conditions.</summary>
public class AlertRule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public Guid CreatedByUserId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string ConditionType { get; set; } = string.Empty;     // "citation_rate_drop", "score_drop", "gap_found", "competitor_rank_change"
    public string ConditionValue { get; set; } = string.Empty;    // Threshold value or percentage
    public string ConditionOperator { get; set; } = string.Empty; // "gt", "lt", "eq", "gte", "lte"

    public string NotificationMethod { get; set; } = string.Empty; // "email", "in_app", "slack"
    public string Frequency { get; set; } = string.Empty;          // "immediate", "daily", "weekly"

    public bool IsActive { get; set; } = true;
    public DateTime? LastTriggeredAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public BusinessProfile Business { get; set; } = null!;
    public User CreatedBy { get; set; } = null!;
}
