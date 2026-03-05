using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Core.DTO
{
    public record UserDTO(Guid UserID, string? Email, string? PersonName, string Gender);
    
}
