using RabbitMQTest.Domain;

namespace RabbitMQTest.Infrastructure.QueueManager.Interfaces;

public interface IProducer
{

    public Task SendProductAlert(ProductMessage productMessage, string exchange, string routingKey);
}