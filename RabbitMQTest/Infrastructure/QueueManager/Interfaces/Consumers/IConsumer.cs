using RabbitMQTest.Domain;

namespace RabbitMQTest.Infrastructure.QueueManager.Interfaces.Consumers
{
    public interface IConsumer
    {
        public Task<ProductMessage> GetAlert();
        public Task SetQueue();
    }
}
