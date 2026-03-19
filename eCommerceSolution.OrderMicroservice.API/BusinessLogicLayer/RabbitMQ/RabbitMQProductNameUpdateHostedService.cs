using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQProductNameUpdateHostedService : IHostedService
    {
        private readonly IRabbitMQProductNameUpdateConsumer _consumer;

        public RabbitMQProductNameUpdateHostedService(IRabbitMQProductNameUpdateConsumer consumer)
        {
             _consumer = consumer;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
             _consumer.Consume();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Dispose();
            return Task.CompletedTask;
        }
    }
}
