using MediatR;

namespace App.Core.Commands
{
    public interface ICommandHandler<in T>: IRequestHandler<T>
        where T : ICommand
    {
    }
}
