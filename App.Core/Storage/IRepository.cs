using System;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Aggregates;

namespace App.Core.Storage
{
    public interface IRepository<T> where T : IAggregate
    {
        Task<T> Find(Guid id, CancellationToken cancellationToken);

        Task Add(T aggregate, CancellationToken cancellationToken);

        Task Update(T aggregate, CancellationToken cancellationToken);

        Task Delete(T aggregate, CancellationToken cancellationToken);
    }
}
