using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces.Consumers;

namespace RabbitMQTest.Infrastructure.QueueManager.RabbitMQ.Consumers
{
    public class RabbitMQDatabaseConsumer(IServiceProvider serviceProvider, ILogger<RabbitMQDatabaseConsumer> logger) : BackgroundService, IDatabaseConsumer
    {
        public string QueueName => "dev.purchases";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = new RabbitMQConnection(serviceProvider.GetRequiredService<IOptions<RabbitMQConfiguration>>());
            await connection.InitializeAsync();

            var consumer = new AsyncEventingBasicConsumer(connection.Channel!);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                logger.LogInformation("Received {message} in database", message);

                await Task.CompletedTask;
            };
            await connection.Channel!.BasicConsumeAsync(QueueName, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);
        }
    }
}
