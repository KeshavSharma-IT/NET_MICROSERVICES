using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.BusinessLogicLayer.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        void Publish<T>(string routingKey,T message);
        
    }
}
