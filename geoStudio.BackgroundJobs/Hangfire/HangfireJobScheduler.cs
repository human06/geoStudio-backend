using Hangfire;

namespace geoStudio.BackgroundJobs.Hangfire;

/// <summary>
/// Centralized registry of all Hangfire recurring jobs.
/// Call <see cref="ScheduleRecurringJobs"/> once after the application is built
/// (after <c>UseHangfireDashboard</c>) to register or update every scheduled job.
/// </summary>
public static class HangfireJobScheduler
{
    /// <summary>Registers all recurring Hangfire jobs. Safe to call on every startup.</summary>
    public static void ScheduleRecurringJobs()
    {
        // ── Audit jobs ────────────────────────────────────────────────────────
        // Triggers the AI visibility audit pipeline for all subscribed businesses.
        RecurringJob.AddOrUpdate(
            recurringJobId: "trigger-recurring-audits",
            methodCall: () => Console.WriteLine("Trigger recurring audits"),
            cronExpression: Cron.Daily(hour: 1),       // 01:00 UTC daily
            options: new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

        // ── Maintenance jobs ──────────────────────────────────────────────────
        // Cleans up completed/failed audit records older than the retention window.
        RecurringJob.AddOrUpdate(
            recurringJobId: "cleanup-old-audits",
            methodCall: () => Console.WriteLine("Cleanup old audits"),
            cronExpression: Cron.Monthly(day: 1, hour: 2), // 02:00 UTC on 1st of month
            options: new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

        // Removes expired refresh tokens from the Users table.
        RecurringJob.AddOrUpdate(
            recurringJobId: "cleanup-expired-tokens",
            methodCall: () => Console.WriteLine("Cleanup expired refresh tokens"),
            cronExpression: Cron.Daily(hour: 3),       // 03:00 UTC daily
            options: new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
    }
}
