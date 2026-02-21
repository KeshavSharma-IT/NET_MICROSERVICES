using eCommerce.OrderMicroservice.BusinessLogicLayer;
using eCommerce.OrderMicroservice.DataAccessLayer;
using OrderMicroservice.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// add services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
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




app.Run();
