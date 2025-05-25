using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQTest.Domain.Shop;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces.Consumers;
using RabbitMQTest.Infrastructure.QueueManager.RabbitMQ;
using RabbitMQTest.Infrastructure.QueueManager.RabbitMQ.Consumers;
using RabbitMQTest.Presentation.ConsoleApp;

namespace RabbitMQTest;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Services.Configure<RabbitMQConfiguration>(
            builder.Configuration.GetSection("RabbitMQ"));

        builder.Services.AddScoped<IShop, Shop>();

        builder.Services.AddSingleton<IQueueConnection, RabbitMQConnection>();

        builder.Services.AddScoped<IProducer, RabbitMQProducer>();

        builder.Services.AddScoped<IDatabaseConsumer, RabbitMQDatabaseConsumer>();
        builder.Services.AddScoped<INotificationConsumer, RabbitMQNotificationConsumer>();
        builder.Services.AddScoped<ILoggerConsumer, RabbitMQLoggerConsumer>();

        builder.Services.AddSingleton<ConsoleManager>();

        using var app = builder.Build();

        await app.Services.GetRequiredService<IQueueConnection>().InitializeAsync();
        await app.Services.GetRequiredService<ConsoleManager>().RunAsync();
    }
}