using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace App.Core.ExternalConsumer.RabbitMq
{
    public class RabbitMqConsumerConfig
    {
        public string HostName { get; set; }

        public string[] Queue { get; set; }
    }

    public static class RabbitMqConsumerConfigExtensions
    {
        public const string DefaultConfigKey = "RabbitMqConsumer";

        public static RabbitMqConsumerConfig GetRabbitConsumerConfig(this IConfiguration configuration)
        {
            return configuration.GetSection(DefaultConfigKey).Get<RabbitMqConsumerConfig>();
        }
    }
}
