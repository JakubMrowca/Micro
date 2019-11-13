using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;

namespace KafkaWork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var config = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("group.id","3"),
                new KeyValuePair<string, string>("bootstrap.servers", "localhost:9092"),
                new KeyValuePair<string, string>("enable.auto.commit", "true"),
                new KeyValuePair<string, string>("auto.offset.reset", "latest")
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe("newKuba");
                try
                {
                    while (true)
                    {
                        // consume event from Kafka
                        var message = consumer.Consume();
                        consumer.Commit();
                        Console.WriteLine(message.Value + " " +message.Key);
                    }
                }
                catch (Exception e)
                {
                    consumer.Close();
                }
            }
        }
    }
}
