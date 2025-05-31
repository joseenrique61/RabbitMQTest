using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest.Infrastructure.QueueManager.Azure
{
    public class AzureSBConfiguration
    {
        public string ProducerConnectionString { get; set; }
        public string ReceiverConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
