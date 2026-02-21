
using eCommerce.DataAccessLayer.Context;
using eCommerce.DataAccessLayer.IRepository;
using eCommerce.DataAccessLayer.Repositiory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,IConfiguration configuration) {

            // Add data access Layer services into the ioc container

            string ConnectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
            string connectionstring= ConnectionStringTemplate.Replace("$MYSQL_HOST",Environment.GetEnvironmentVariable("MYSQL_HOST"))
            .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySQL(connectionstring);
            });


            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
