using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DTO
{
    public record ProductResponce(Guid ProductId, string ProductName, CategoryOptions Category, double? UnitPrice, int? QuentityInStock)
    {
        public ProductResponce() : this(default, default, default, default, default) { }

    }
}
