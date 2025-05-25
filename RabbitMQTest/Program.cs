using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQTest.Presentation.ConsoleApp;

namespace RabbitMQTest;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<ConsoleManager>();

        using var app = builder.Build();

        await app.Services.GetRequiredService<ConsoleManager>().RunAsync();
    }
}