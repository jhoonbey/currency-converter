using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CurrencyConverter.Extensions;

public static class SwaggerServicesExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Currency Converter API",
                Version = "v1",
                Description = "An API to perform Currency Conversion operations",
                Contact = new OpenApiContact
                {
                    Name = "Currency Converter",
                },
                License = new OpenApiLicense
                {
                    Name = "Currency Converter API",
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerUi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            options.DocumentTitle = "Currency Converter Swagger";
            options.DocExpansion(DocExpansion.List);
            options.DefaultModelsExpandDepth(1);
        });

        return app;
    }
}