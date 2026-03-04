using eCommerce.OrderMicroservice.DataAccessLayer.Repositories;
using eCommerce.OrderMicroservice.DataAccessLayer.ReposittoryContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace eCommerce.OrderMicroservice.DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // add access later service into ioc containerw
            string connectionStringTemplate = configuration.GetConnectionString("MongoDB")!;
            string connectionString = connectionStringTemplate.Replace("$MONGO_HOST", Environment.GetEnvironmentVariable("MONGODB_HOST"))
                .Replace("$MONGO_PORT", Environment.GetEnvironmentVariable("MONGODB_PORT"));


            services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
            services.AddScoped<IMongoDatabase>(provider =>
            {
                IMongoClient client=  provider.GetRequiredService<IMongoClient>();
                return client.GetDatabase("OrdersDatabase");
            });

            services.AddScoped<IOrdersRepository, OrdersRepositiory>();


            return services;
        }
    }
}
