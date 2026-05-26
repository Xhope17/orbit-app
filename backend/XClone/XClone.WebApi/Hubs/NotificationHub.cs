using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using XClone.Shared.Constants;

namespace XClone.WebApi.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimsConstants.USER_ID)?.Value;
        if (userId is not null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"notifications_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimsConstants.USER_ID)?.Value;
        if (userId is not null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"notifications_{userId}");
        }
        await base.OnDisconnectedAsync(exception);
    }
}
