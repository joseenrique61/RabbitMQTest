using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQTest.Domain;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;
using RabbitMQTest.Infrastructure.QueueManager.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQTest.Infrastructure.QueueManager.Azure
{
    public class AzureSBProducer(ILogger<AzureSBProducer> logger, IServiceProvider serviceProvider) : IProducer
    {
        public async Task SendProductAlert(ProductMessage productMessage, string exchange, string routingKey)
        {
            try
            {
                var connection = new AzureSBConnection(serviceProvider.GetRequiredService<IOptions<AzureSBConfiguration>>());

                connection.InitializeAsync();

                await using var client = new ServiceBusClient(connection.ProducerConnectionString);

                connection.Sender = client.CreateSender(connection.QueueName);
                connection.Message = new ServiceBusMessage(JsonSerializer.Serialize(productMessage));

                await connection.Sender.SendMessageAsync(connection.Message);
                
                logger.LogInformation("Sent product alert to exchange: {exchange} with routing key: {routingKey}",
                    exchange, routingKey);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }
    }
}