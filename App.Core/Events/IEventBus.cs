using System.Threading.Tasks;

namespace App.Core.Events
{
    public interface IEventBus
    {
        Task Publish(params IEvent[] events);
    }
}
