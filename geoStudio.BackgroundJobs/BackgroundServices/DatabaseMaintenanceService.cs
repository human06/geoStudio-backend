using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace geoStudio.BackgroundJobs.BackgroundServices;

/// <summary>Hosted service that periodically performs database housekeeping tasks.</summary>
public sealed class DatabaseMaintenanceService(ILogger<DatabaseMaintenanceService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("DatabaseMaintenanceService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogDebug("Running database maintenance...");
                // TODO: purge expired refresh tokens, old notifications, etc.
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during database maintenance.");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        logger.LogInformation("DatabaseMaintenanceService stopping.");
    }
}
