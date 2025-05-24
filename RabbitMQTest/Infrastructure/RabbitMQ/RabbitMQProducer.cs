using RabbitMQ.Client;
using RabbitMQTest.Domain;

namespace RabbitMQTest.Infrastructure.RabbitMQ;

public class RabbitMQProducer
{
    public static void SendAlert(AlertMessage alertMessage)
    {
        var message = new AlertMessage(alertMessage.Message, alertMessage.Binding);

    }
}