using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Events
{
    public class FrontendEvent:IEvent
    {
        public string ConnectionId { get; set; }
    }
}
