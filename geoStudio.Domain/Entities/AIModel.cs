namespace geoStudio.Domain.Entities;

/// <summary>Represents a specific AI model offered by a provider.</summary>
public class AIModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProviderId { get; set; }
    public string ModelName { get; set; } = string.Empty;        // "GPT-4.5", "Gemini 2.0", "Claude 3.5 Sonnet"
    public string ModelIdentifier { get; set; } = string.Empty;  // "gpt-4.5-turbo", "gemini-2.0-flash" (for API calls)
    public string? Version { get; set; }                          // "4.5", "2.0", "3.5"
    public string? DisplayName { get; set; }                      // "GPT-4.5 Turbo", "Gemini 2.0 Flash"
    public string? Description { get; set; }

    // Pricing
    public decimal CostPer1kInputTokens { get; set; }
    public decimal CostPer1kOutputTokens { get; set; }

    // Configuration
    public int MaxTokens { get; set; }
    public bool SupportsStreaming { get; set; } = true;
    public bool SupportsVision { get; set; } = false;

    // Open Source Information
    public bool IsOpenSource { get; set; } = false;
    public string? ModelUrl { get; set; }  // GitHub, HuggingFace, or official docs URL

    // Status
    public bool IsActive { get; set; } = true;
    public bool IsAvailableForTesting { get; set; } = true;
    public DateTime? LaunchDate { get; set; }
    public DateTime? DeprecationDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public AIProvider Provider { get; set; } = null!;
    public ICollection<AuditResult> AuditResults { get; set; } = [];
}
