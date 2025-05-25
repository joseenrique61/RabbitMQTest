using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RabbitMQTest.Infrastructure.RabbitMQ;

internal class RabbitMQConnection
{
    public static RabbitMQConnection? Instance { get; private set; }

    private readonly IConfiguration _configuration = new ConfigurationManager();

    public IChannel? Channel { get; private set; }

    private RabbitMQConnection() { }

    public async Task<RabbitMQConnection> Init()
    {
        if (Instance == null)
        {
            Instance = new RabbitMQConnection();
            await CreateConnection();

            return Instance;
        }      
        
        return Instance;
    }

    private async Task CreateConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQ:Hostname"]!,
            Port = int.Parse(_configuration["RabbitMQ:Port"]!),
            UserName = _configuration["RabbitMQ:Username"]!,
            Password = _configuration["RabbitMQ:Password"]!
        };
        var connection = await factory.CreateConnectionAsync();
        Channel = await connection.CreateChannelAsync();
    }
}