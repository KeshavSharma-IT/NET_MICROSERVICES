using   Ecommerce.Infrastructure;
using   Ecommerce.Core;
using Ecommerce.Api.Middleware;
using System.Text.Json.Serialization;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddCore();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);

var app = builder.Build();

//Exception Handling middleware
app.UseExceptionHandlingMiddleware();


//routing
app.UseRouting();

//auth
app.UseAuthentication();
app.UseAuthorization();

// Routhing for controller
app.MapControllers();

app.Run();
