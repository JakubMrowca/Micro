using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Core.ExternalConsumer;
using MediatR;
using User.Shared.Events;

namespace User.Services.EventHandlers
{
    public class UserValidEventHandler:INotificationHandler<UserValid>
    {
        private readonly IExternalEventProducer _externalEventProducer;
        public UserValidEventHandler(IExternalEventProducer externalEventProducer)
        {
            _externalEventProducer = externalEventProducer;
        }

        public async Task Handle(UserValid notification, CancellationToken cancellationToken)
        {

            //await _externalEventProducer.Publish(notification);
        }
    }
}
