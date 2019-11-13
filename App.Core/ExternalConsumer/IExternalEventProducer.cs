using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App.Core.Commands;
using App.Core.Events;

namespace App.Core.ExternalConsumer
{
    public interface IExternalEventProducer
    {
        Task Publish(IExternalEvent @event);
    }
}
