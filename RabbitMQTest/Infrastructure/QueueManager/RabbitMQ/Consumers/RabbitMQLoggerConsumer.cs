using RabbitMQTest.Domain;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces.Consumers;

namespace RabbitMQTest.Infrastructure.QueueManager.RabbitMQ.Consumers
{
    public class RabbitMQLoggerConsumer : ILoggerConsumer
    {
        public Task<ProductMessage> GetAlert()
        {
            throw new NotImplementedException();
        }

        public Task SetQueue()
        {
            throw new NotImplementedException();
        }
    }
}
