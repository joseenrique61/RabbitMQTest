using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTest.Domain;
using RabbitMQTest.Domain.Shop;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces.Consumers;

namespace RabbitMQTest.Infrastructure.QueueManager.RabbitMQ.Consumers
{
    public class RabbitMQNotificationConsumer(IServiceProvider serviceProvider, ILogger<RabbitMQNotificationConsumer> logger, IShop shop) : BackgroundService, IDatabaseConsumer
    {
        public string QueueName => "dev.notifications";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = new RabbitMQConnection(serviceProvider.GetRequiredService<IOptions<RabbitMQConfiguration>>());
            await connection.InitializeAsync();

            var consumer = new AsyncEventingBasicConsumer(connection.Channel!);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                logger.LogInformation("Received {message} in notifications", message);

                try
                {
                    var productMessage = JsonSerializer.Deserialize<ProductMessage>(message)!;
                    var product = shop.Products.First(x => x.Id == productMessage.Id);
                    Console.WriteLine($"[x] Notification: your order for '{product.Name}' was processed");
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    await Task.FromException(e);
                    return;
                }

                await Task.CompletedTask;
            };
            await connection.Channel!.BasicConsumeAsync(QueueName, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);
        }
    }
}
