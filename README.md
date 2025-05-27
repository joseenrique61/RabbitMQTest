# RabbitMQTest
RabbitMQTest is our project for testing RabbitMQ, a queue manager, with a real use case.
The use case we chose is _buying a product online_.

# Running the container
To run the container, use the following command:
```
sudo docker run -d -it --name rabbitmq --hostname 4cfb3f5e6ca7 -p 5672:5672 -p 15672:15672 -v route_to_rabbitmq_volume:/var/lib/rabbitmq rabbitmq:3.12-management
```

# Explanation
## Logging
The logging is made with Serilog, with the packages:
- [Serilog](https://www.nuget.org/packages/Serilog/4.3.0)
- [Serilog.Extensions.Hosting](https://www.nuget.org/packages/Serilog.Extensions.Hosting/9.0.0)
- [Serilog.Settings.Configuration](https://www.nuget.org/packages/Serilog.Settings.Configuration/9.0.0)
- [Serilog.Sinks.File](https://www.nuget.org/packages/Serilog.Sinks.File/7.0.0)

All logging is written to a file called logs/log.txt

## Configuration
### RabbitMQ
RabbitMQ is deployed with docker. The version used is 3.12-management. The package used for connecting with the app is [RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client/7.1.2).

#### Users
There are two users created: guest (default) and normal_user, that only has access to the virtual host _dev_.

#### Virtual hosts
There are two virtual hosts created: / (default) and dev, the one used for this project.

#### Exchanges
- amq.*: Default exchanges
- dev.direct: Exchange of type direct
- dev.fanout: Exchange of type fanout
- dev.topic: Exchange of type topic

#### Queues
- dev.logger: is binded to dev.topic with routing key '*.purchase'
- dev.notifications: is binded to dev.topic with routing key '*.purchase', and dev.direct with routing key 'notifications'
- dev.purchases: is binded to dev.direct with routing key 'database'

### App
Configuration of the app is made through a file called `appsettings.json`. It follows the format of:
```
{
    "Logging": {
        "LogLevel": {
            "Default": "Debug",
            "System": "Information",
            "Microsoft": "Information"
        }
    },
    "RabbitMQ": {
        "HostName": "",
        "Port": 0,
        "VirtualHost": "",
        "Username": "",
        "Password": ""
    }
}
```
In the repository there is a file called appsettings-example.json which has this format for editing.

## How does the app work?
We used .NET Core 9 for the project. It is a CLI application.

It uses Dependency Injection (DI) for achieving Inversion of Control (IoC).
This is done through a package called [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/9.0.5).
This is an official package from Microsoft, which provides the tools for managing dependencies, logging and configuration.

### CLI
The console is managed through a service called ConsoleManager. It calls the producer through an interface to send messages to RabbitMQ.

### Producer
The producer is a dependency injected into the ConsoleManager. It has a method for sending messages to RabbitMQ, in which you can specify the message, exchange and routing key for the message. It creates a connection and a channel with the specified information in the configuration.

### Consumers
Consumers, as the producer, create a connection and a channel with the specified information in the configuration. One connection is created per producer or consumer, as it is not recommendable to use the same for all in an asynchronous environment.

#### Database consumer
The database consumer is subscribed to the dev.purchases queue. It logs that it received a message and writes to a file called db/db.txt the product received.

#### Notification consumer
The notification consumer is subscribed to the dev.notifications queue. It logs that it received a message and writes to the console a message that a order was processed.

#### Logger consumer
The logger consumer is subscribed to the dev.notifications queue. It logs that it received a message.