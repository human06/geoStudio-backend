namespace geoStudio.Domain.Entities;

/// <summary>Represents a generated report for an audit.</summary>
public class Report
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AuditId { get; set; }
    public Guid CreatedByUserId { get; set; }

    public string ReportType { get; set; } = string.Empty; // "executive", "detailed", "comparative"
    public string Title { get; set; } = string.Empty;
    public string? ContentJson { get; set; }
    public string? PdfUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Audit Audit { get; set; } = null!;
    public User CreatedBy { get; set; } = null!;
}
