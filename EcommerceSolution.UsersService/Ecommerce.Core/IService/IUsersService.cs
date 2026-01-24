using Ecommerce.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Core.IService
{
    public  interface IUsersService
    {
        Task<AuthenticationResponse?>  Login(LoginRequest request);
        Task<AuthenticationResponse?>  Register(RegisterRequest request);
    }
}
