using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using RabbitMQTest.Domain;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;
using RabbitMQTest.Infrastructure.ServiceBus.AzureServiceBus.Interfaces;

namespace RabbitMQTest.Infrastructure.ServiceBus.AzureServiceBus;

public class AzureProducer(ILogger<AzureProducer> logger, IServiceBusConnectionProducer serviceBusConnectionProducer) : IProducer
{
    public async Task SendProductAlert(ProductMessage productMessage, string queue, string routingKey)
    {
        var sender = serviceBusConnectionProducer.ServiceBusClient.CreateSender(queue);
        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

        if (!messageBatch.TryAddMessage(new ServiceBusMessage(JsonSerializer.Serialize(productMessage))))
        {
            throw new Exception($"The message is too large to fit in the batch.");
        }

        try
        {
            await sender.SendMessagesAsync(messageBatch);
            logger.LogInformation("Sent message to queue");
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }
}