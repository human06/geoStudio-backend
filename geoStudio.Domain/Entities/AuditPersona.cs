namespace geoStudio.Domain.Entities;

/// <summary>Join entity linking a Persona to an Audit run.</summary>
public class AuditPersona
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AuditId { get; set; }
    public Guid PersonaId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Audit Audit { get; set; } = null!;
    public Persona Persona { get; set; } = null!;
}
