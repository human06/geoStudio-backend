namespace geoStudio.Domain.Entities;

/// <summary>Represents a scheduled recurring report for a business.</summary>
public class ReportSchedule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public Guid CreatedByUserId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;  // "executive", "detailed", "comparative"
    public string Frequency { get; set; } = string.Empty;   // "daily", "weekly", "monthly", "quarterly"
    public int? DayOfWeek { get; set; }     // 0-6 (Sunday-Saturday)
    public int? DayOfMonth { get; set; }    // 1-31
    public TimeSpan? TimeOfDay { get; set; }
    public string? RecipientEmails { get; set; }  // JSON array or comma-separated

    public bool IsActive { get; set; } = true;
    public DateTime? LastRunAt { get; set; }
    public DateTime? NextRunAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public BusinessProfile Business { get; set; } = null!;
    public User CreatedBy { get; set; } = null!;
}
