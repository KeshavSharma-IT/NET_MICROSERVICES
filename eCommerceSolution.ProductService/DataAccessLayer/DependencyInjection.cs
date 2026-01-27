
using DataAccessLayer.Context;
using DataAccessLayer.IRepository;
using DataAccessLayer.Repositiory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,IConfiguration configuration) {

            // Add data access Layer services into the ioc container

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
            });


            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
