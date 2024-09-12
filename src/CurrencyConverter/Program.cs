using System.Text;
using CurrencyConverter.Extensions;
using CurrencyConverter.Middleware;

Console.OutputEncoding = Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(options => { options.AllowEmptyInputInBodyModelBinding = true; });
builder.Services.AddSwagger();
builder.Services.AddMapper();
builder.Services.AddRefit(config);
builder.Services.AddRedis(config);
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseSwaggerUi();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();