using App.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace User.Shared.Command
{
    public class ValidUser: ICommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConnectionId { get; set; }

    }
}
