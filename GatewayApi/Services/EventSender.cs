using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Events;
using App.SignalR.Hubs;
using Gateway.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace GatewayApi.Services
{
    public interface IEventSender
    {
        Task Send(UserValid @event);
    }
    public class EventSender:IEventSender
    {
        private readonly IHubContext<UserHub> _hub;

        public EventSender(IHubContext<UserHub> hub)
        {
            _hub = hub;
        }

        public async Task Send(UserValid @event)
        {
            var obj = new object[2];
            obj[0] = @event.GetType().Name;
            obj[1] = @event;

            await _hub.Clients.Client(@event.ConnectionId).SendCoreAsync("EventEmited", obj);
        }
    }
}
