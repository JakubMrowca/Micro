using System;
using System.Collections.Generic;
using App.Core.Events;

namespace App.Core.Aggregates
{
    public interface IAggregate
    {
        Guid Id { get; }
        int Version { get; }

        IEnumerable<IEvent> DequeueUncommittedEvents();
    }
}
