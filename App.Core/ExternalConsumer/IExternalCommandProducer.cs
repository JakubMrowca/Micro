using System.Threading.Tasks;
using App.Core.Commands;

namespace App.Core.ExternalConsumer
{
    public interface IExternalCommandProducer
    {
        Task Publish(IExternalCommand command);
    }
}