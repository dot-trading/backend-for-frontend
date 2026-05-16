using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TradingProject.Bff.Client.Configuration;
using TradingProject.Bff.Client.Services;

namespace TradingProject.Bff.Client;

/// <summary>
/// Extension methods for registering the BFF API client services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the <see cref="IBffApiClient"/> with its <see cref="HttpClient"/> and configuration.
    /// </summary>
    /// <param name="services">The dependency injection container.</param>
    /// <param name="configuration">The application configuration (used to bind <c>"BffApi"</c> section).</param>
    /// <returns>The same service collection for chaining.</returns>
    /// <exception cref="OptionsValidationException">Thrown at start-up if configuration is missing or invalid.</exception>
    public static IServiceCollection AddBffApiClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind & validate options
        services.AddOptions<BffApiClientOptions>()
            .Bind(configuration.GetSection(BffApiClientOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Register typed HttpClient
        services.AddHttpClient<IBffApiClient, BffApiClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<BffApiClientOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(options.ApiKey))
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", options.ApiKey);
            }
        });

        return services;
    }
}
