using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace App.Core.ExternalConsumer.RabbitMq
{
    public class RabbitMqProducerConfig
    {
        public string HostName { get; set; }
        public string Topic { get; set; }
    }

    public static class KafkaProducerConfigExtensions
    {
        public const string DefaultConfigKey = "RabbitMqProducer";

        public static RabbitMqProducerConfig GetRabbitProducerConfig(this IConfiguration configuration)
        {
            return configuration.GetSection(DefaultConfigKey).Get<RabbitMqProducerConfig>();
        }
    }
}
