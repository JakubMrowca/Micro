using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Commands;
using App.Core.Events;
using MediatR;
using User.Services.Repositories.Read;
using User.Shared.Command;
using User.Shared.Events;

namespace User.Services.CommandHandlers
{
    public class ValidUserCommandHandler:ICommandHandler<ValidUser>
    {
        private readonly UsersReadRepository _usersReadRepository;
        private readonly IEventBus _eventBus;

        public ValidUserCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _usersReadRepository = new UsersReadRepository();
        }

        public async Task<Unit> Handle(ValidUser command, CancellationToken cancellationToken)
        {
            var user = _usersReadRepository.GetUser(command.UserName);
            if(user == null)
                throw new Exception("user Not exist");
            await _eventBus.Publish(new UserValid {TimeStamp = DateTime.Now, User = user,ConnectionId = command.ConnectionId});
            return new Unit();
        }
    }
}
