namespace geoStudio.Domain.Entities;

/// <summary>Represents the AI response result for a single query/persona/model combination in an audit.</summary>
public class AuditResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AuditId { get; set; }
    public Guid QueryId { get; set; }
    public Guid PersonaId { get; set; }
    public Guid AIModelId { get; set; }

    public bool IsMentioned { get; set; }
    public int? RankingPosition { get; set; }
    public int CompetitorMentions { get; set; }
    public string? CitationSnippet { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Audit Audit { get; set; } = null!;
    public AuditQuery Query { get; set; } = null!;
    public Persona Persona { get; set; } = null!;
    public AIModel AIModel { get; set; } = null!;
}
