using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Commands;
using App.Core.Events;
using App.Core.Reflection;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace App.Core.ExternalConsumer.RabbitMq
{
    public class RabbitMqConsumer : IExternalEventConsumer
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly RabbitMqConsumerConfig config;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqConsumer(
            IServiceProvider serviceProvider,
            ILogger<RabbitMqConsumer> logger,
            IConfiguration configuration
        )
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // get configuration from appsettings.json
            config = configuration.GetRabbitConsumerConfig();
            InitRabbitMQ();
        }
        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = config.HostName,UserName = "rabbitmq", Password = "rabbitmq" };

            // create connection
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rabbit consumer started");
            foreach (var queue in config.Queue)
            {
                _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                    consumer.Received += (ch, ea) =>
                    {
                        // received message
                        var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                        // handle the received message
                        HandleMessage(content,queue);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    };

                    consumer.Shutdown += OnConsumerShutdown;
                    consumer.Registered += OnConsumerRegistered;
                    consumer.Unregistered += OnConsumerUnregistered;
                    consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

                    _channel.BasicConsume(queue, false, consumer);
                //}
            }
        }

        private void HandleMessage(string content, string queue)
        {
            try
            {
                var commandType = TypeProvider.GetTypeFromAnyReferencingAssembly(queue);

                // deserialize event
                var command = JsonConvert.DeserializeObject(content, commandType);

                using (var scope = serviceProvider.CreateScope())
                {
                    var commandBus =
                        scope.ServiceProvider.GetRequiredService<ICommandBus>();

                    // publish event to internal event bus
                    commandBus.Send(command as ICommand);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error consuming message: " + e.Message + e.StackTrace);
            }
            // we just print this message   
            _logger.LogInformation($"consumer received {content}");
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }
    }
}
