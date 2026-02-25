namespace geoStudio.Domain.Enums;

/// <summary>Represents the lifecycle state of an AI visibility audit.</summary>
public enum AuditStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Failed = 3,
    Cancelled = 4
}
