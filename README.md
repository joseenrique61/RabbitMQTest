# Ariel Anchapaxi
This is a C# application that sends purchase data to an Azure Service Bus, specifically to a queue named test-queue.

# Azure Service Bus
Azure Service Bus is a service that provides the alternative to manage a big quantity of queues that allows sent and receive messages.

# How it works?
To use the Azure SB we need to use the string connections provides by Azure. When we have the string connection, we use it into Visual Studio to configure the connection.

Each time a purchase is made in the application, a Purchase object is serialized to JSON and sent to the test-queue in Azure Service Bus. This approach decouples the purchasing logic from downstream processing such as invoicing, inventory management, or analytics.

# Features
- Secure connection to Azure Service Bus
- Sends messages containing purchase information
- Utilizes Dependency Injection for clean and maintainable architecture
- Uses a secret JSON configuration file to store Azure credentials

# Technologies Used
- .NET Core
- Azure.Messaging.ServiceBus
- Dependency Injection
- JSON Configuration

# Secret Configuration
The `appsettings.json` file stores sensitive configuration like the Azure Service Bus connection string and queue name. This file should not be committed to the repository.

"Logging": {
    "LogLevel": {
        "Default": "Debug",
        "System": "Information",
        "Microsoft": "Information"
    }
},
"Azure": {
  "ConnectionString": "",
  "QueueName": ""
}

# Explanation
## Logging
The logging is made with Serilog, with the packages:
- [Serilog](https://www.nuget.org/packages/Serilog/4.3.0)
- [Serilog.Extensions.Hosting](https://www.nuget.org/packages/Serilog.Extensions.Hosting/9.0.0)
- [Serilog.Settings.Configuration](https://www.nuget.org/packages/Serilog.Settings.Configuration/9.0.0)
- [Serilog.Sinks.File](https://www.nuget.org/packages/Serilog.Sinks.File/7.0.0)

All logging is written to a file called logs/log.txt

## Configuration

### CLI
The console is managed through a service called ConsoleManager. It calls the producer through an interface to send messages to RabbitMQ.

### Producer
The producer is a dependency injected into the ConsoleManager. It has a method for sending messages to Azure, in which you can specify the message, exchange and routing key for the message. It creates a connection and a channel with the specified information in the configuration.