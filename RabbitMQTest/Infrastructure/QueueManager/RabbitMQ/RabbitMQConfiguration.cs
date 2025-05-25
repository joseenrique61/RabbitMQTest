namespace RabbitMQTest.Infrastructure.QueueManager.RabbitMQ;

public class RabbitMQConfiguration
{
    public string HostName { get; set; }

    public int Port { get; set; }

    public string VirtualHost { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
}