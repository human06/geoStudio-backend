namespace geoStudio.BackgroundJobs.Jobs;

/// <summary>Defines the contract for running AI visibility audits as background jobs.</summary>
public interface IAuditJob
{
    /// <summary>Executes the audit pipeline for the given audit record.</summary>
    Task ExecuteAsync(Guid auditId, CancellationToken cancellationToken = default);
}
