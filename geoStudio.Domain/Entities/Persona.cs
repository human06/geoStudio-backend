namespace geoStudio.Domain.Entities;

/// <summary>Represents a target customer persona used to tailor audit queries.</summary>
public class Persona
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCustom { get; set; } = false;
    public bool IsPublic { get; set; } = true;

    // Demographics
    public string? AgeRange { get; set; }        // "18-25", "26-35", etc.
    public string? LocationType { get; set; }    // "urban", "suburban", "rural"
    public string? IncomeLevel { get; set; }     // "low", "middle", "high", "luxury"
    public string? EducationLevel { get; set; }  // "high_school", "bachelor", "master", "phd"
    public string? TechSavviness { get; set; }   // "novice", "intermediate", "advanced"

    public string? SearchIntent { get; set; }
    public string? PainPoints { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public BusinessProfile Business { get; set; } = null!;
    public ICollection<AuditPersona> AuditPersonas { get; set; } = [];
    public ICollection<AuditResult> AuditResults { get; set; } = [];
}
