using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Core.IRepository
{
    public interface IUsersRepository
    {
        Task<ApplicationUser?> AddUser(ApplicationUser user);

        Task<ApplicationUser?> GetUserByEmailAndPassword (string email, string password);
    }
}
