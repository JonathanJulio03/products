using Products.Api.Application.Port.In;
using Products.Api.Application.Port.Out;
using Products.Api.Application.Service;
using Products.Api.Infrastructure.Middleware;
using Products.Api.Infrastructure.Out.Adapter.External;
using Products.Api.Infrastructure.Out.Adapter.Persistence;
using Products.Api.Infrastructure.Out.Adapter.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<IWeatherPort, WeatherAdapter>(client => 
{
    client.BaseAddress = new Uri("https://api.weatherapi.com/v1/");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

builder.Services.AddScoped<IProductPort, ProductAdapter>();
builder.Services.AddScoped<IProductUseCase, ProductService>();
builder.Services.AddScoped<IWeatherUseCase, WeatherService>();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Type = null;
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionMiddleware>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

await app.RunAsync();