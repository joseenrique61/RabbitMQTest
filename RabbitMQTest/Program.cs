using Microsoft.Extensions.Hosting;

namespace RabbitMQTest;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder();



        using var host = builder.Build();
    }
}