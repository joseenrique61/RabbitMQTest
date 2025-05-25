    using RabbitMQ.Client.Events;
    using RabbitMQTest.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    namespace RabbitMQTest.Infrastructure.RabbitMQ
    {
    public class RabbitMQConsumer
    {
        public async Task<ReceivedMessage> GetAlert()
        {
            var connection = await RabbitMQConnection.Instance!.Init();

            var tcs = new TaskCompletionSource<ReceivedMessage>();

            var consumer = new AsyncEventingBasicConsumer(connection.Channel!);
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
    }

}
