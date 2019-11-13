using System;
using App.Core.Events;
using Gateway.Shared.Models;

namespace Gateway.Shared.Events
{
    public class UserValid:FrontendEvent
    {
        public DateTime TimeStamp { get; set; }
        public UserVm User { get; set; }

    }
}
