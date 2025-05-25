using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQTest.Domain;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;

namespace RabbitMQTest.Infrastructure.QueueManager.RabbitMQ;

public class RabbitMQProducer(ILogger<RabbitMQProducer> logger, IQueueConnection connection) : IProducer
{
    public async Task SendProductAlert(ProductMessage productMessage, string exchange, string routingKey)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(productMessage));

            await connection.Channel!.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                body: body
            );

            logger.LogInformation(" [x] Sent product alert to exchange: {exchange} with routing key: {routingKey}",
                exchange, routingKey);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }
    }
}