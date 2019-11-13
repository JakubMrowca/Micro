using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.ExternalConsumer
{
    public interface IExternalEventConsumer
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}
