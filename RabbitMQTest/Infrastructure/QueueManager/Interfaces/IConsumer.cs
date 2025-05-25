using RabbitMQTest.Domain;
using RabbitMQTest.Infrastructure.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest.Infrastructure.QueueManager.Interfaces
{
    public interface IConsumer
    {
        public Task<ReceivedMessage> GetAlert();
        public Task SetQueue();
    }
}
