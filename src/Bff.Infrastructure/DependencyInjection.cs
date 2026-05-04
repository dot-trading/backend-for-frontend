using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bff.Domain.Abstractions;
using Bff.Infrastructure.Options;
using Bff.Infrastructure.Services;

namespace Bff.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BinanceOptions>(configuration.GetSection("Binance"));
        services.Configure<ClusterOptions>(configuration.GetSection("Cluster"));
        services.Configure<PersistenceOptions>(configuration.GetSection("Persistence"));
        services.Configure<ThirdPartyOptions>(configuration.GetSection("ThirdParty"));

        services.AddHttpClient<IDatabaseService, DatabaseService>();
        services.AddHttpClient<IBinanceService, BinanceService>();
        services.AddScoped<IClusterService, ClusterService>();

        return services;
    }
}
