
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

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
            });


            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
