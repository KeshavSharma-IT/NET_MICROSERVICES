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
            string query = "INSERT INTO public.\"Users\"(\"UserID\", \"Email\", \"PersonName\", \"Gender\", \"Password\") VALUES(@UserID, @Email, @PersonName, @Gender, @Password)";
            int rowCountAffected = await _dbContext.DbConnection.ExecuteAsync(query, user);

            if (rowCountAffected > 0) 
            { 
                return user;
            }

            return null;
        }

        public async Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password)
        {
            //sql query to select a user  by email id and password
            string query = "SELECT * FROM public.\"Users\" Where \"Email\"=@Email AND \"Password\"=@Password";

    //        var user = await _dbContext.DbConnection
    //.QueryFirstOrDefaultAsync<ApplicationUser>(query, new { Email = email, Password = password });
            var parameters = new { Email=email,Password = password};
           ApplicationUser? user =await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

           return user;

            //return new ApplicationUser() {
            //    Email = email,
            //    Password = password  ,
            //    PersonName="KESHAV",
            //    UserID=Guid.NewGuid(),
            //    Gender=GenderOptions.Male.ToString(),

            //};
        }
    }
}
