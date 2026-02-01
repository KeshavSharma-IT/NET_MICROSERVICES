using eCommerce.BusinessLogicLayer;
using eCommerce.DataAccessLayer;
using eCommerce.ProductMicroService.Api.ApiEndPoints;
using eCommerce.ProductMicroService.Api.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//add Dal and Bll services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();

//swagger
app.UseSwagger();
app.UseSwaggerUI();

// auth
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.MapProductApiEndPoints();

app.Run();

