using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQTest.Domain;
using RabbitMQTest.Domain.Shop;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces.Consumers;
using RabbitMQTest.Infrastructure.ServiceBus.AzureServiceBus.Interfaces;

namespace RabbitMQTest.Infrastructure.ServiceBus.AzureServiceBus;

public class AzureDatabaseConsumer(ILogger<AzureDatabaseConsumer> logger, IServiceBusConnectionConsumer serviceBusConnectionConsumer, IShop shop) : BackgroundService, IDatabaseConsumer
{
    public string QueueName => "purchases";


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processor = serviceBusConnectionConsumer.ServiceBusClient.CreateProcessor(QueueName, new ServiceBusProcessorOptions());

        try
        {
            processor.ProcessMessageAsync += async (args) =>
            {
                var body = args.Message.Body.ToString();
                var productMessage = JsonSerializer.Deserialize<ProductMessage>(body)!;
                var product = shop.Products.First(x => x.Id == productMessage.Id);
                await File.AppendAllTextAsync("db/db.txt", JsonSerializer.Serialize(product) + "\n", stoppingToken);

                logger.LogInformation("Received {message} in database", body);

                await args.CompleteMessageAsync(args.Message, stoppingToken);
            };

            processor.ProcessErrorAsync += (args) =>
            {
                logger.LogError(args.Exception.ToString());
                return Task.CompletedTask;
            };

            await processor.StartProcessingAsync(stoppingToken);

            // await Task.Delay(60000, stoppingToken);
            //
            // await processor.StopProcessingAsync(stoppingToken);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }
        // finally
        // {
        //     await processor.DisposeAsync();
        // }
    }
}