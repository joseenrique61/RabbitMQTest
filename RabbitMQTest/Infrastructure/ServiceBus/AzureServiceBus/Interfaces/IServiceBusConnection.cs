using Azure.Messaging.ServiceBus;

namespace RabbitMQTest.Infrastructure.ServiceBus.AzureServiceBus.Interfaces;

public interface IServiceBusConnection
{
    public ServiceBusClient ServiceBusClient { get; }
}