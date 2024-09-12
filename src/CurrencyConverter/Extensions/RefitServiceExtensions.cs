using CurrencyConverter.Constants;
using CurrencyConverter.Services;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Refit;

namespace CurrencyConverter.Extensions;

public static class RefitServiceExtensions
{
    public static IServiceCollection AddRefit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<IFrankfurterService>()
            .ConfigureHttpClient((_, c) => { c.BaseAddress = new Uri(configuration["Frankfurter:ApiUrl"]!); })
            .AddResilienceHandler("resilience", resilienceBuilder =>
            {
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = ApplicationConstants.MaxAPIRetryCount,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .HandleResult(response => !response.IsSuccessStatusCode)
                });

                resilienceBuilder.AddTimeout(TimeSpan.FromSeconds(5));
            });

        return services;
    }
}