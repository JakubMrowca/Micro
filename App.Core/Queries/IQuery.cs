using MediatR;

namespace App.Core.Queries
{
    public interface IQuery<out TResponse>: IRequest<TResponse>
    {
    }
}
