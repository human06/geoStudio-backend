namespace geoStudio.Domain.Entities;

/// <summary>Represents a competitor tracked by a business.</summary>
public class Competitor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? WebsiteUrl { get; set; }
    public string? IndustryCategory { get; set; }
    public DateTime? LastChecked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public BusinessProfile Business { get; set; } = null!;
}
