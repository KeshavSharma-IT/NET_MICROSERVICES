using   Ecommerce.Infrastructure;
using   Ecommerce.Core;
using Ecommerce.Api.Middleware;
using System.Text.Json.Serialization;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Mappers;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddCore();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);

//FluentValidation
//builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });


//swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


//cors services
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
var app = builder.Build();

//Exception Handling middleware
app.UseExceptionHandlingMiddleware();


//routing
app.UseRouting();


//swagger end point

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

//auth
app.UseAuthentication();
app.UseAuthorization();

// Routhing for controller
app.MapControllers();

app.Run();
