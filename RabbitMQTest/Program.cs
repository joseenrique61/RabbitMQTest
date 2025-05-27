using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQTest.Domain.Shop;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;
using RabbitMQTest.Infrastructure.QueueManager.RabbitMQ;
using RabbitMQTest.Infrastructure.QueueManager.RabbitMQ.Consumers;
using RabbitMQTest.Presentation.ConsoleApp;
using Serilog;

namespace RabbitMQTest;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSerilog((_, configuration) =>
        {
            configuration.WriteTo.File("logs/logs.txt", outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:l}{NewLine}");
        });

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Services.Configure<RabbitMQConfiguration>(
            builder.Configuration.GetSection("RabbitMQ"));

        builder.Logging.AddConsole();

        builder.Services.AddScoped<IShop, Shop>();

        builder.Services.AddScoped<IProducer, RabbitMQProducer>();

        builder.Services.AddHostedService<RabbitMQDatabaseConsumer>();
        builder.Services.AddHostedService<RabbitMQNotificationConsumer>();
        builder.Services.AddHostedService<RabbitMQLoggerConsumer>();

        builder.Services.AddHostedService<ConsoleManager>();

        using var app = builder.Build();

        await app.RunAsync();
    }
}