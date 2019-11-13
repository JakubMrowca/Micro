using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace GatewayApi.Consumer
{
    public class KafkaConsumer:IHostedService, IDisposable
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Hello World!");
            var config = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("group.id","1"),
                new KeyValuePair<string, string>("bootstrap.servers", "localhost:9092"),
                new KeyValuePair<string, string>("enable.auto.commit", "true"),
                new KeyValuePair<string, string>("auto.offset.reset", "latest")
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe("workTopic");
                try
                {
                    while (true)
                    {
                        // consume event from Kafka
                        var message = consumer.Consume();
                        consumer.Commit();
                        Console.WriteLine(message.Value);
                    }
                }
                catch (Exception e)
                {
                    consumer.Close();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
