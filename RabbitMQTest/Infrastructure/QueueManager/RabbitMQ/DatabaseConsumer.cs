using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTest.Domain;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace RabbitMQTest.Infrastructure.RabbitMQ
    {
    public class RabbitMQDatabaseConsumer : IConsumer
    {
        private RabbitMQConnection? _connection;
        public async Task<ReceivedMessage> GetAlert()
        {
            _connection = await RabbitMQConnection.Instance!.Init();

            await SetQueue();

            var tcs = new TaskCompletionSource<ReceivedMessage>();

            var consumer = new AsyncEventingBasicConsumer(_connection.Channel!);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var receivedMessage = new ReceivedMessage(message);
                Console.WriteLine($" [x] Received {message}");

                tcs.SetResult(receivedMessage);

                await Task.CompletedTask;
            };

            return await tcs.Task;
        }

        private async Task SetQueue()
        {
            string queue = "dev.purchases";
            await _connection!.Channel!.QueueDeclareAsync(queue: queue);
        }
    }
}
