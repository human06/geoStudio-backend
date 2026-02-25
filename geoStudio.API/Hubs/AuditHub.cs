using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace geoStudio.API.Hubs;

/// <summary>SignalR hub for real-time audit progress updates.</summary>
[Authorize]
public sealed class AuditHub : Hub
{
    /// <summary>Join a business-specific group to receive audit notifications.</summary>
    public async Task JoinBusinessGroup(string businessId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"business-{businessId}");

    /// <summary>Leave a business-specific group.</summary>
    public async Task LeaveBusinessGroup(string businessId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"business-{businessId}");
}
