using geoStudio.Domain.Enums;

namespace geoStudio.Domain.Entities;

/// <summary>Represents a user's membership within a business team.</summary>
public class TeamMember
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BusinessId { get; set; }
    public Guid UserId { get; set; }
    public UserRole Role { get; set; } = UserRole.Viewer;
    public Guid? InvitedByUserId { get; set; }
    public DateTime InvitedAt { get; set; } = DateTime.UtcNow;
    public DateTime? JoinedAt { get; set; }
    public TeamMemberStatus Status { get; set; } = TeamMemberStatus.Invited;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public BusinessProfile Business { get; set; } = null!;
    public User User { get; set; } = null!;
    public User? InvitedByUser { get; set; }
}
