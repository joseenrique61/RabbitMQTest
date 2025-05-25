using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTest.Domain;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;

namespace RabbitMQTest.Infrastructure.QueueManager.RabbitMQ.Consumers
    {
    public class RabbitMQDatabaseConsumer(IQueueConnection connection) : IDatabaseConsumer
    {
        public async Task<ProductMessage> GetAlert()
        {
            await SetQueue();

            var tcs = new TaskCompletionSource<ProductMessage>();

            var consumer = new AsyncEventingBasicConsumer(connection.Channel!);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var productMessage = JsonSerializer.Deserialize<ProductMessage>(message)!;
                Console.WriteLine($" [x] Received {message}");

                tcs.SetResult(productMessage);

                await Task.CompletedTask;
            };

            return await tcs.Task;
        }

        public async Task SetQueue()
        {
            // string queue = "dev.purchases";
            // await connection!.Channel!.QueueDeclareAsync(queue: queue);
        }
    }
}
