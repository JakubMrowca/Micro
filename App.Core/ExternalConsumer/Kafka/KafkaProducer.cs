using System;
using System.Threading.Tasks;
using App.Core.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace App.Core.ExternalConsumer.Kafka
{
    public class KafkaProducer: IExternalEventProducer
    {
        private readonly KafkaProducerConfig config;

        public KafkaProducer(
            IConfiguration configuration
        )
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            config = configuration.GetKafkaProducerConfig();
        }

        public async Task Publish(IExternalEvent @event)
        {
            using (var p = new ProducerBuilder<string, string>(config.ProducerConfig).Build())
            {
                // publish event to kafka topic taken from config
                await p.ProduceAsync(config.Topic,
                    new Message<string, string>
                    {
                        // store event type name in message Key
                        Key = @event.GetType().Name,
                        // serialize event to message Value
                        Value = JsonConvert.SerializeObject(@event)
                    });
            }
        }
    }
}
