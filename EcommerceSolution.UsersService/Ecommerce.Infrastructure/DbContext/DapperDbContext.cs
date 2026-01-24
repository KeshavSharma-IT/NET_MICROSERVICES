using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ecommerce.Infrastructure.DbContext
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;
        public DapperDbContext(IConfiguration configuration) {
           
           _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("Postgres");

            //create NpgSql connection with the retrieved connection string

            _connection= new NpgsqlConnection(connectionString);
        }


        public IDbConnection DbConnection=>_connection;

    }
}
