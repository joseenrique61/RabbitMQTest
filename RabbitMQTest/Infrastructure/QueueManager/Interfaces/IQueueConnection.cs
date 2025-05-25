using RabbitMQ.Client;

namespace RabbitMQTest.Infrastructure.QueueManager.Interfaces;

public interface IQueueConnection
{
    public Task InitializeAsync();

    public IChannel? Channel { get; }
}