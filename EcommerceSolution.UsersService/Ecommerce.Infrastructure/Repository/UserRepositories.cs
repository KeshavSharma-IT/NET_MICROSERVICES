using Dapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;
using Ecommerce.Core.IRepository;
using Ecommerce.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Infrastructure.Repository
{
    public class UserRepositories : IUsersRepository
    {
        private readonly DapperDbContext _dbContext;
        public UserRepositories(DapperDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task<ApplicationUser?> AddUser(ApplicationUser user)
        {
            //Generate Guid
            user.UserID = Guid.NewGuid();

            //sql query to insert user data into the "Users" table
            string query = "INSERT INTO public .\"Users\" (\"UserID\",\"Email\",,\"PersonName\",\"Gender\",,\"Password\") VALUES(@UserID,@Email,@PersonName,@Gender,@Pasword)";

            int rowCountAffected =await _dbContext.DbConnection.ExecuteAsync(query,user);

            if (rowCountAffected > 0) 
            { 
                return user;
            }

            return null;
        }

        public async Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password)
        {
            return new ApplicationUser() {
                Email = email,
                Password = password  ,
                PersonName="KESHAV",
                UserID=Guid.NewGuid(),
                Gender=GenderOptions.Male.ToString(),

            };
        }
    }
}
