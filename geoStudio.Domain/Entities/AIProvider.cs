namespace geoStudio.Domain.Entities;

/// <summary>Represents an AI provider (OpenAI, Google, Anthropic, etc.).</summary>
public class AIProvider
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;         // "openai", "google", "anthropic", "meta"
    public string DisplayName { get; set; } = string.Empty;  // "OpenAI", "Google DeepMind", "Anthropic"
    public string? WebsiteUrl { get; set; }
    public string? ApiEndpoint { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<AIModel> Models { get; set; } = [];
}
