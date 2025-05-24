using RabbitMQ.Client;
using RabbitMQTest.Domain;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQTest.Infrastructure.RabbitMQ;

public class RabbitMQProducer
{
    public async static void SendAlert(AlertMessage alertMessage)
    {
        var connection = await RabbitMQConnection.Instance!.Init();


        await connection.Channel!.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: alertMessage.Binding,
            body: Encoding.UTF8.GetBytes(alertMessage.Message)
            );

        Console.WriteLine($" [x] Sent {alertMessage.Message}");
    }
}