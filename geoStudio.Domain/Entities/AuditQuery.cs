namespace geoStudio.Domain.Entities;

/// <summary>Represents a search query used within an audit to test AI visibility.</summary>
public class AuditQuery
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AuditId { get; set; }
    public string QueryText { get; set; } = string.Empty;
    public int? SearchVolume { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Audit Audit { get; set; } = null!;
    public ICollection<AuditResult> Results { get; set; } = [];
}
