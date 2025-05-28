using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using RabbitMQTest.Infrastructure.ServiceBus.AzureServiceBus.Interfaces;

namespace RabbitMQTest.Infrastructure.ServiceBus.AzureServiceBus;

public class AzureConnectionConsumer : IServiceBusConnectionConsumer
{
    public ServiceBusClient ServiceBusClient { get; }

    public AzureConnectionConsumer(IOptions<AzureConfiguration> options)
    {
        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        ServiceBusClient = new ServiceBusClient(options.Value.ConnectionStringConsumer, clientOptions);
    }
}