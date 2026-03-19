using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.RabbitMQ
{
    public record ProductNameUpdateMessage(Guid ProductID, string? NewName);
}
