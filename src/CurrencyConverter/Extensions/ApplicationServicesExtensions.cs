using CurrencyConverter.Services;

namespace CurrencyConverter.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyConversionService, CurrencyConversionService>();
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}