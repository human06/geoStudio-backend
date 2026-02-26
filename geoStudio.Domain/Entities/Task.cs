namespace geoStudio.Domain.Entities;

/// <summary>Represents an actionable improvement task for a business.</summary>
public class Task
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "not_started";  // "not_started", "in_progress", "completed", "on_hold"
    public string Priority { get; set; } = "medium";      // "high", "medium", "low"
    public int? EffortEstimateHours { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? RelatedGapId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public BusinessProfile Business { get; set; } = null!;
    public User CreatedBy { get; set; } = null!;
    public User? AssignedTo { get; set; }
}
