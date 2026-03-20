using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQProductDeletionHostedService : IHostedService
    {
        private readonly IRabbitMQProductDeletionConsumer _consumer;

        public RabbitMQProductDeletionHostedService(IRabbitMQProductDeletionConsumer consumer)
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
