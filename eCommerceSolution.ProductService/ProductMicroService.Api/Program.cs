using BusinessLogicLayer;
using DataAccessLayer;
using ProductMicroService.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

//add Dal and Bll services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();


var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();


// auth
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Run();
