using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DTO
{
   
    public record ProductUpdateRequest(Guid ProductId,string ProductName, CategoryOptions Category, double? UnitPrice, int? QuentityInStock)
    {
        public ProductUpdateRequest() : this(default,default, default, default, default) { }

    }
}
