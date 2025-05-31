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
        private readonly AzureSBConfiguration _configuration = options!.Value!;
        public string QueueName {  get; private set; }
        public string ProducerConnectionString { get; private set; }
        public string ReceiverConnectionString { get; private set; }
        public ServiceBusSender Sender { get; set; }
        public ServiceBusReceiver Receiver { get; set; }
        public ServiceBusMessage Message { get; set; }
        public ServiceBusReceivedMessage ReceivedMessage { get; set; }

        //public AzureSBConfiguration Instance { get; set; } = this;
        public void InitializeAsync()
        {
            ProducerConnectionString = _configuration.ProducerConnectionString;
            ReceiverConnectionString = _configuration.ReceiverConnectionString;

            QueueName = _configuration.QueueName;
        }
    }
}
