using RabbitMQTest.Infrastructure.QueueManager.Interfaces.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Azure.Messaging.ServiceBus;
using System.Diagnostics;

namespace RabbitMQTest.Infrastructure.QueueManager.Azure.Consumers
{
    public class AzureSBConsumer(IServiceProvider serviceProvider, ILogger<AzureSBConsumer> logger) : BackgroundService, IDatabaseConsumer
    {
        public string QueueName => "";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                ServiceBusProcessor _processor;
                ServiceBusClient _client;

                var connection = new AzureSBConnection(serviceProvider.GetRequiredService<IOptions<AzureSBConfiguration>>());

                connection.InitializeAsync();

                //await using var client = new ServiceBusClient(connection.ConnectionString);

                var clientOptions = new ServiceBusClientOptions()
                {
                    TransportType = ServiceBusTransportType.AmqpWebSockets
                };

                _client = new ServiceBusClient(connection.ReceiverConnectionString, clientOptions);
                _processor = _client.CreateProcessor(connection.QueueName, new ServiceBusProcessorOptions());

                try
                {
                    _processor.ProcessMessageAsync += MessageHandler;

                    _processor.ProcessErrorAsync += ErrorHandler;

                    // start processing 
                    await _processor.StartProcessingAsync();

                    Console.WriteLine("Wait for a minute and then press any key to end the processing");
                    Console.ReadKey();

                    // stop processing 
                    Console.WriteLine("\nStopping the receiver...");
                    await _processor.StopProcessingAsync();
                    Console.WriteLine("Stopped receiving messages");
                }
                finally
                {
                    // Calling DisposeAsync on client types is required to ensure that network
                    // resources and other unmanaged objects are properly cleaned up.
                    await _processor.DisposeAsync();
                    await _processor.DisposeAsync();
                }

                //connection.Receiver = _client.CreateReceiver(connection.QueueName);
                //connection.ReceivedMessage = await connection.Receiver.ReceiveMessageAsync();

                //string body = connection.ReceivedMessage.Body.ToString();
                //Console.WriteLine(body);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
