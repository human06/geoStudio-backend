namespace geoStudio.API.Hubs;

public static class SignalRExtensions
{
    /// <summary>
    /// Registers SignalR with tuned message size and keep-alive settings.
    /// </summary>
    public static IServiceCollection AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            options.MaximumReceiveMessageSize = 64 * 1024; // 64 KB
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        });

        return services;
    }
}
