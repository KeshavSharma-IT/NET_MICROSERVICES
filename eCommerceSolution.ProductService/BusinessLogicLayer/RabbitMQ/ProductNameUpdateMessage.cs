using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.BusinessLogicLayer.RabbitMQ
{
    public record ProductNameUpdateMessage(Guid ProductID, string? NewName);
    
}
