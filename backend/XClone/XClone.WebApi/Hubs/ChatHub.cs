using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using XClone.Shared.Constants;

namespace XClone.WebApi.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimsConstants.USER_ID)?.Value;
        if (userId is not null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimsConstants.USER_ID)?.Value;
        if (userId is not null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}
