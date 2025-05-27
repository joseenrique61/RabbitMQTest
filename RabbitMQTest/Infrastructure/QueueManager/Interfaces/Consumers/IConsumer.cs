using RabbitMQTest.Domain;

namespace RabbitMQTest.Infrastructure.QueueManager.Interfaces.Consumers
{
    public interface IConsumer
    {
        public string QueueName { get; }
    }
}
