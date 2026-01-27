using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DTO
{
    public record ProductAddRequest(string ProductName,CategoryOptions Category,double? UnitPrice,int? QuentityInStock)
    {
        public ProductAddRequest()  :this(default,default,default,default) { }
        
    }
    
}
