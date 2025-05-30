# RabbitMQTest
RabbitMQTest is our project for testing RabbitMQ, in this case adapted to Azure, with a real use case.
The use case we chose is _buying a product online_.

# Explanation
## Logging
The logging is made with Serilog, with the packages:
- [Serilog](https://www.nuget.org/packages/Serilog/4.3.0)
- [Serilog.Extensions.Hosting](https://www.nuget.org/packages/Serilog.Extensions.Hosting/9.0.0)
- [Serilog.Settings.Configuration](https://www.nuget.org/packages/Serilog.Settings.Configuration/9.0.0)
- [Serilog.Sinks.File](https://www.nuget.org/packages/Serilog.Sinks.File/7.0.0)

All logging is written to a file called logs/log.txt

## Configuration
### Azure Service Bus
Azure Service Bus was configured through the [Azure Portal](https://portal.azure.com/#home).

#### Queues
There is one queue configured for the app: *_purchases_*. This queue is in charge of managing the purchases made by the customer.

#### Shared Access Policies
There are two shared access policies, one for listening to the queue, _ListenPruchaseEvents_, and another for sending messages to it, _SendPurchasesEvents_.

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
    },
    "AzureServiceBus": {
        "ConnectionStringProducer": "",
        "ConnectionStringConsumer": ""
    }
}
```
In the repository there is a file called _appsettings-example.json_ which has this format for editing.

## How does the app work?
We used .NET Core 9 for the project. It is a CLI application.

It uses Dependency Injection (DI) for achieving Inversion of Control (IoC).
This is done through a package called [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/9.0.5).
This is an official package from Microsoft, which provides the tools for managing dependencies, logging and configuration.

### CLI
The console is managed through a service called ConsoleManager. It calls the producer through an interface to send messages to the Azure Service Bus.

### Connection
There are two classes that manage connections to the Service Bus. There is one for the producer, _AzureConnectionProducer_, and other for the consumer, _AzureConnectionConsumer_. This is done so each one has their own connection string.

### Producer
The producer is a dependency injected into the ConsoleManager. It has a method for sending messages to Azure Service Bus, in which you can specify the message and queue for the message. It uses the connection provided by the AzureConnectionProducer.

#### Database consumer
The database consumer is a service that works on the background, so that it doesn't block the main thread waiting for the messages.

It is subscribed to the purchases queue. It uses the connection provided by AzureConnectionConsumer and opens a processor (an object that reads the messages from the queue) and closes it when the service is shut down. 

It logs that it received a message and writes to a file called db/db.txt the product received.
