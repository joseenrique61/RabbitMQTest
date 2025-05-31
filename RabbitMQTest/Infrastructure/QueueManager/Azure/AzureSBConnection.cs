using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQTest.Infrastructure.QueueManager.Interfaces;
using RabbitMQTest.Infrastructure.QueueManager.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest.Infrastructure.QueueManager.Azure
{
    public class AzureSBConnection(IOptions<AzureSBConfiguration> options)
    {
        private readonly AzureSBConfiguration _configuration = options.Value;
        public string QueueName {  get; set; }
        public string ConnectionString { get; set; }
        public ServiceBusSender Sender { get; set; }
        public ServiceBusMessage Message { get; set; }      

        public void Initialize()
        {
            ConnectionString = _configuration.ConnectionString;
            QueueName = _configuration.QueueName;
        }
    }
}
