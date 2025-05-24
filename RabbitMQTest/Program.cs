using RabbitMQTest.Infrastructure.RabbitMQ;

namespace RabbitMQTest;

class Program
{
    async static void Main(string[] args)
    {
        var connection = RabbitMQConnection.Instance;

        await connection.Init();

        var channel = connection.Channel;



    }
}