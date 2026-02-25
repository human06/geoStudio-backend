using Microsoft.AspNetCore.SignalR;

namespace geoStudio.API.Hubs;

/// <summary>SignalR hub for real-time audit progress tracking.</summary>
public class AuditProgressHub : Hub
{
    private readonly ILogger<AuditProgressHub> _logger;

    public AuditProgressHub(ILogger<AuditProgressHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client {ConnectionId} connected to AuditProgressHub", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client {ConnectionId} disconnected from AuditProgressHub", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>Subscribe to progress events for a specific audit.</summary>
    public async Task JoinAuditProgress(string auditId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"audit-{auditId}");
        _logger.LogInformation("Client {ConnectionId} joined group audit-{AuditId}", Context.ConnectionId, auditId);
    }

    /// <summary>Unsubscribe from progress events for a specific audit.</summary>
    public async Task LeaveAuditProgress(string auditId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"audit-{auditId}");
        _logger.LogInformation("Client {ConnectionId} left group audit-{AuditId}", Context.ConnectionId, auditId);
    }
}
