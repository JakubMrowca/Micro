using System;
using System.Text;
using System.Threading.Tasks;
using App.Core.Commands;
using App.Core.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace App.Core.ExternalConsumer.RabbitMq
{
    public class RabbitMqProducer : IExternalCommandProducer
    {
        private readonly RabbitMqProducerConfig config;

        public RabbitMqProducer(
            IConfiguration configuration
        )
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            config = configuration.GetRabbitProducerConfig();

        }

        public async Task Publish(IExternalCommand command)
        {
            var factory = new ConnectionFactory() { HostName = config.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: command.GetType().Name, durable: true, exclusive: false, autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
                channel.BasicPublish(exchange: "", routingKey: command.GetType().Name, basicProperties: null, body: body);
            }
            //_channel.BasicPublish("logs", routingKey: "Commands.ValidUser", basicProperties: null, body: body);
        }
    }
}
