using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace App.SignalR.Hubs
{
    public class UserHub : Hub
    {
        public async Task InitHubUser(string user, string message)
        {
            var connectionId = Context.ConnectionId;
            await Clients.Client(connectionId).SendAsync("HubUserInited", user, connectionId);
        }

        public async Task SendResponse(string toUser, string response)
        {

        }
    }
}
