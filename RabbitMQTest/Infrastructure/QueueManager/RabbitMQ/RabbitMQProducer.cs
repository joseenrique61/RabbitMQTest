using System.Text;
using RabbitMQ.Client;
using RabbitMQTest.Domain;

namespace RabbitMQTest.Infrastructure.QueueManager.RabbitMQ;

public class RabbitMQProducer
{
    public async static void SendAlert(ProductMessage productMessage)
    {
        var connection = await RabbitMQConnection.Instance!.Init();

        await connection.Channel!.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: productMessage.routingKey,
            body: Encoding.UTF8.GetBytes(new StringBuilder().Append(productMessage.id.ToString()).Append(productMessage.name).Append(productMessage.price).ToString())
            );

        Console.WriteLine($" [x] Sent");
    }
}