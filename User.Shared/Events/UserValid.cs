using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Events;
using User.Shared.Dto;

namespace User.Shared.Events
{
    public class UserValid:IExternalEvent
    {
        public DateTime TimeStamp { get; set; }
        public UserVm User { get; set; }
        public string ConnectionId { get; set; }
    }
}
