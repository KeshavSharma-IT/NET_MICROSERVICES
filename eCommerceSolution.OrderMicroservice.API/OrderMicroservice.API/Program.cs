using eCommerce.OrderMicroservice.BusinessLogicLayer;
using eCommerce.OrderMicroservice.BusinessLogicLayer.HttpClients;
using eCommerce.OrderMicroservice.BusinessLogicLayer.Policies;
using eCommerce.OrderMicroservice.DataAccessLayer;
using OrderMicroservice.API.Middleware;
using Polly;

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

builder.Services.AddTransient<IUserMicroServicePolicies, UserMicroservicesPolicies>();
builder.Services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();
builder.Services.AddTransient<IPollyPolicies, PollyPolicies>();

builder.Services.AddHttpClient<UserMicroserviceClient>(client =>
{
    client.BaseAddress = new Uri($"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}/");
})
//.AddPolicyHandler(
//      builder.Services.BuildServiceProvider().GetRequiredService<IUserMicroServicePolicies>().GetRetryPolicy()
//    )
//.AddPolicyHandler(
//      builder.Services.BuildServiceProvider().GetRequiredService<IUserMicroServicePolicies>().GetCircuitBreakerPolicy()
//    ).AddPolicyHandler(
//      builder.Services.BuildServiceProvider().GetRequiredService<IUserMicroServicePolicies>().GetTimeoutPolicy()
//    )
    .AddPolicyHandler(
      builder.Services.BuildServiceProvider().GetRequiredService<IUserMicroServicePolicies>().GetCombinedPolicy()
    );

builder.Services.AddHttpClient<ProductsMicroserviceClient>(client =>
{
    client.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}/");
}).AddPolicyHandler(
      builder.Services.BuildServiceProvider().GetRequiredService<IProductsMicroservicePolicies>().GetFallBackPolicy()
      ).AddPolicyHandler(
      builder.Services.BuildServiceProvider().GetRequiredService<IProductsMicroservicePolicies>().GetBulkheadIsolationPolicyPolicy()
      );

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();

//swagger
app.UseSwagger();
app.UseSwaggerUI();

// auth
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();




app.Run();
