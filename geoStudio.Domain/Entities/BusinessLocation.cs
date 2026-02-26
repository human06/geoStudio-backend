namespace geoStudio.Domain.Entities;

/// <summary>Represents a physical or virtual location belonging to a business.</summary>
public class BusinessLocation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? WebsiteUrl { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int? ServiceRadiusKm { get; set; }
    public bool EnabledForTesting { get; set; } = true;
    public bool IsPrimary { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public BusinessProfile Business { get; set; } = null!;
    public ICollection<Audit> Audits { get; set; } = [];
}
