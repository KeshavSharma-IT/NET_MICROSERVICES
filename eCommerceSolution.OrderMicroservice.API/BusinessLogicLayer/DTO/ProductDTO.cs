using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.DTO
{
    public record ProductDTO(Guid ProductsID, string? productName, string? category, double unitPrice,int quantityInStock);
}
