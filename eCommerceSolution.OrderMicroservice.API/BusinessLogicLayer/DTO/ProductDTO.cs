using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.DTO
{
    public record ProductDTO(Guid ProductID, string? ProductName, string? Category, double UnitPrice,int QuantityInStock);
}
