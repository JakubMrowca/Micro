using System;
using Confluent.Kafka;

namespace KafkaBus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var config = new ProducerConfig {BootstrapServers = "localhost:9092" };
            Action<DeliveryReport<Null, string>> handler = r =>
                {
                    Console.WriteLine(!r.Error.IsError
                        ? $"Delivered message to {r.TopicPartitionOffset}"
                        : $"Delivery error: {r.Error.Reason}");
                };
            using (var producer = new ProducerBuilder<Null,string>(config).Build())
            {
                var stringValue = "";
                while (stringValue != "-")
                {
                    stringValue = Console.ReadLine();
                    producer.ProduceAsync("newKuba", new Message<Null, string>() { Value = stringValue });
                }
                producer.Flush(TimeSpan.FromSeconds(10));
            }

        }
    }
}
