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
        public async ProductMessage GetAlert()
        {
            var connection = await RabbitMQConnection.Instance!.Init();

            var consumer = new AsyncEventingBasicConsumer(connection.Channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");
                return Task.CompletedTask;
            };


        }
    }
}
