using App.Core.Commands;

namespace Gateway.Shared.Commands
{
    public class ValidUser:IExternalCommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConnectionId { get; set; }
    }
}
