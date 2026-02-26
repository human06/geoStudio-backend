using geoStudio.Domain.Enums;

namespace geoStudio.Domain.Entities;

/// <summary>Represents an AI visibility audit run for a business.</summary>
public class Audit
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? CreatedByUserId { get; set; }

    public AuditStatus Status { get; set; } = AuditStatus.Pending;

    // Metrics (populated after audit completes)
    public decimal? VisibilityScore { get; set; }
    public decimal? CitationRate { get; set; }
    public int? AverageRankingPosition { get; set; }
    public int? TotalMentions { get; set; }
    public int? TotalTestsCount { get; set; }
    public int? CompletedTestsCount { get; set; }

    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public BusinessProfile Business { get; set; } = null!;
    public BusinessLocation? Location { get; set; }
    public User? CreatedBy { get; set; }
    public ICollection<AuditQuery> Queries { get; set; } = [];
    public ICollection<AuditPersona> Personas { get; set; } = [];
    public ICollection<AuditResult> Results { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
}
