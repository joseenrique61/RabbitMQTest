using RabbitMQTest.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest.Infrastructure.QueueManager.Interfaces
{
    public interface IRabbitMQConsumer
    {
        public Task<ReceivedMessage> GetAlert();
        public Task GetQueue();
    }
}
